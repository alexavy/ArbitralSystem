
#'
#' Cerberus: monitoring arbitrage opportunities
#'


# Import dependencies ----

options(max.print = 1e3, scipen = 999, width = 1e2)
options(stringsAsFactors = F)

suppressPackageStartupMessages({
  library(dplyr)
  library(tibble)
  library(tidyr)
  library(magrittr)
  library(purrr)
  library(data.table)
  
  library(odbc)
  
  library(stringr)
  library(lubridate)
  
  library(skimr)
  library(tictoc)
  
  library(ggplot2)
})

source("core-experiment.R")
theme_set(theme_bw())



# Get configs and set params ----

job <- list(
  config = config::get(),
  secrets = config::get(file = "secrets.yml")
)

stopifnot(
  is.list(job),
  !is.null(job$config)
)

print(job$config)



# Load dataset ----

## Open DB connections
market_info_db_con <- dbConnect(odbc::odbc(), .connection_string = job$secrets$market_info_conn_string)
public_market_data_db_con <- dbConnect(odbc::odbc(), .connection_string = job$secrets$public_market_data_conn_string)


## Get history
orderbook_timestamps_dt <- dbGetQuery(market_info_db_con, "select * from orderbook_timestamps_vw")
orderbook_timestamps_dt %<>% normalize_col_names

stopifnot(
  nrow(orderbook_timestamps_dt) > 0,
  nrow(orderbook_timestamps_dt) == n_unique(orderbook_timestamps_dt$symbol)
)


orderbook_timestamps_dt %>% 
  select(symbol, ends_with("_timestamp")) %>% 
  pivot_longer(cols = ends_with("_timestamp")) %>% 
  
  ggplot(aes(x = value, y = symbol)) +
    geom_line()



# Functions ----

## Get orderbook for specific pair

get_settings <- function(dt) {
  stopifnot(
    is.data.frame(dt),
    nrow(dt) == 1
  )
  
  list(
    symbol = dt[1, ]$symbol,
    from_time = dt[1, ]$last_orderbook_timestamp - minutes(job$config$depth_in_minutes),
    to_time = dt[1, ]$last_orderbook_timestamp
  )
}


#' Execute SQL procedure
#'
#' @param .con 
#' @param .proc_name 
#' @param .settings 
#'
exec_proc <- function(.con, .proc_name, .settings) {
  stopifnot(
    is.character(.proc_name),
    is.list(.settings) && length(.settings) == 3
  )
  
  dbGetQuery(.con, 
             sprintf("exec %s @symbol = '%s', @fromTime = '%s', @toTime = '%s'", .proc_name, .settings$symbol, .settings$from_time, .settings$to_time))
}


#' Round datetime and return timestamp
#'
#' @param x 
#' @param .unit 
#'
to_timestamp <- function(x, .unit = "second") {
  require(lubridate)
  require(dplyr)
  
  x %>% floor_date(.unit) %>% as.integer
}


#' Get orderbooks
#'
#' @param .settings 
#'
get_orderbooks <- function(.settings) {
  
  tic("Executing get_orderbook_sp...")
  dt <- exec_proc(market_info_db_con, "get_orderbook_sp", .settings)
  toc()
  
  dt %>% 
    normalize_col_names
}


#' Get bot statuses
#'
#' @param .settings 
#'
get_bot_statuses <- function(.settings) {
  
  .settings$from_time <- .settings$from_time - months(1) # HACK
  
  tic("Executing get_bot_statuses_history_sp...")
  dt <- exec_proc(market_info_db_con, "get_bot_statuses_history_sp", .settings)
  toc()
  
  
  dt %<>% 
    normalize_col_names %>% 
    #
    mutate(timestamp = to_timestamp(timestamp)) %>% 
    #
    group_by(exchange, symbol, timestamp) %>% 
    summarise(
      status = dplyr::last(status, order_by = timestamp),
      .groups = "drop"
    )
  
  
  if (nrow(dt) == 0) {
    print("[WARN] Records not found")
    
    exchanges_n <- 6L
    
    dt %<>%
      rbind(tibble(
        exchange = 1:exchanges_n,
        symbol = rep(.settings$symbol, exchanges_n),
        status = rep(4L, exchanges_n),
        timestamp = rep(.settings$from_time, exchanges_n)
      ))
  } else {
    print(sprintf(
      "[INFO] Bots detected for following exchanges: %s", paste(unique(dt$exchange), collapse = ", ")
    ))
  }
  
  
  expand.grid(
      exchange = unique(dt$exchange),
      symbol = unique(dt$symbol),
      timestamp = seq(.settings$from_time, .settings$to_time, by = 1) %>% to_timestamp
    ) %>% 
    left_join(
      dt, by = c("exchange", "symbol", "timestamp")
    ) %>% 
    group_by(exchange, symbol) %>% 
    arrange(timestamp) %>% 
    fill(status, .direction = "down") %>% 
    ungroup
}


#' Get exchange rates
#'
#' @param .settings 
#'
get_1D_candles  <- function(.settings) {
  
  tic("Executing get_symbol_price_sp...")
  dt <- exec_proc(public_market_data_db_con, "get_symbol_price_sp", .settings)
  toc()
  
  dt %>% 
    ##
    normalize_col_names %>% 
    mutate(
      symbol = str_to_upper(symbol),
      date = as.Date(timestamp)
    ) %>% 
    
    ## calculate OHLC
    group_by(symbol, exchange, date) %>% 
    summarise(
      open = first(price, order_by = timestamp),
      high = max(price, na.rm = T),
      low = min(price, na.rm = T),
      close = last(price, order_by = timestamp),
      time = max(timestamp),
      
      .groups = "drop"
    )
}



### Preprocessing ----

##

jobs_settings <- orderbook_timestamps_dt %>% 
  ## business rules
  filter(exchanges_n >= job$config$min_exchange_n) %>% 
  ## prepare
  group_by(symbol) %>% 
  group_split %>% 
  ## start
  map(get_settings)


## For each pair in settings 

settings <- jobs_settings[[2]]
settings


orderbooks_dt <- get_orderbooks(settings)

orderbooks_dt %<>% 
  mutate(date = as.Date(timestamp))

stopifnot(
  nrow(orderbooks_dt) > 0,
  !anyNA(orderbooks_dt)
)

orderbooks_dt %>% as_tibble

orderbooks_dt %>% 
  mutate(hour_of_day = hour(timestamp)) %>% 
  count(exchange, symbol, hour_of_day) %>% 
  spread(exchange, n, fill = 0) %>% 
  arrange(hour_of_day)


bot_statuses_dt <- get_bot_statuses(settings)

bot_statuses_dt %<>% 
  filter(!is.na(status)) %>% 
  rename(timestamp_round = timestamp)

stopifnot(
  nrow(bot_statuses_dt) > 0,
  !anyNA(bot_statuses_dt),
  bot_statuses_dt$exchange %>% n_unique > 1,
  bot_statuses_dt %>% distinct(exchange, symbol, timestamp_round) %>% nrow == nrow(bot_statuses_dt)
)

bot_statuses_dt


usdt_settings <- settings
usdt_settings$symbol <- sprintf("%sUSDT", str_split(settings$symbol, "/")[[1]][2]) # TODO: insert / in pair name
stopifnot(
  usdt_settings$symbol != "USDTUSDT"
)

print(usdt_settings)



trading_pair_candles_dt <- get_1D_candles(usdt_settings)

get_min_close_price <- function(.date) {
  print(.date)
  
  trading_pair_candles_dt %>% 
    filter(date == .date) %>% 
    pull(close) %>% 
    min(., na.rm = T)
}


trading_pair_candles_dt %<>% 
  ##
  right_join(
    expand.grid(
      symbol = unique(trading_pair_candles_dt$symbol),
      exchange = orderbooks_dt %>% filter(symbol == settings$symbol) %>% pull(exchange) %>% unique,
      date = orderbooks_dt %>% filter(symbol == settings$symbol) %>% pull(date) %>% unique
    ),
    by = c("symbol", "exchange", "date")
  )
  
trading_pair_candles_dt$min_close <- sapply(trading_pair_candles_dt$date, get_min_close_price)

trading_pair_candles_dt %<>% 
  mutate(
    close = if_else(is.na(close), min_close, close)
  ) %>% 
  select(-min_close)
  

stopifnot(
  nrow(trading_pair_candles_dt) > 0,
  !anyNA(trading_pair_candles_dt %>% select(symbol, exchange, date, close))
)

trading_pair_candles_dt



# Join all together ----

data <- orderbooks_dt %>% 
  ##
  mutate(
    timestamp_round = to_timestamp(timestamp),
    date = as.Date(timestamp)
  ) %>% 
  
  ##
  left_join(
    bot_statuses_dt, 
    by = c("exchange", "symbol", "timestamp_round")
  ) %>% 
  left_join(
    trading_pair_candles_dt %>% transmute(exchange, date, trading_pair_1D_close = close), 
    by = c("exchange", "date")
  ) %>% 
  
  ##
  mutate_at(
    all_of(c("symbol", "exchange", "direction", "status")),
    as.factor
  )

data %>% skim


stopifnot(
  nrow(data) > 0,
  nrow(data) <= nrow(orderbooks_dt),
  !anyNA(data)
)


data %>% 
  count(date, exchange) %>% 
  spread(exchange, n)


print(
  sprintf("[WARN] Missed values for %s seconds", 
          data %>% filter(status != job$config$connected_bot_status) %>% pull(timestamp_round) %>% n_unique)
)

data %<>% filter(status == job$config$connected_bot_status)




# Work ----


## Find arbitrage opportunity

arbitrage_cases <- data %>% 
  
  ## get only quotes w/ arbitrage opportunity
  inner_join(
    trading_pair_candles_dt %>% filter(arbitrage_flag),
    by = c("symbol", "timestamp_round")
  ) %>% 
  
  ## calculate weighted delta
  mutate(
    delta = if_else(direction == 0,
                    price - best_ask, best_bid - price, 
                    NA_real_),
    w_delta = volume*delta*(1 - 2*job$config$exchange_fee_pcnt) # TODO: we need to use min volume 
  ) %>% 
  filter(w_delta > 0) %>% 
  
  ## summarize all deltas in orderbook snapshot
  group_by(
    symbol, timestamp_round, direction
  ) %>% 
  summarise(
    w_delta_total = sum(w_delta)
  ) %>% 
  ungroup %>% 
  
  ## store only min delta
  group_by(
    symbol, timestamp_round
  ) %>% 
  summarise(
    w_delta_total_refined = min(w_delta_total) # TODO: we need to use min volume for ask and bid separately
  ) %>% 
  ungroup %>% 
  
  ## get exchange rate
  mutate(
    usdt_pair = sprintf("%s/USDT", str_split(symbol, "/")[[1]][2])
  ) %>% 
  left_join(
    trading_pair_candles_dt %>% 
      filter(str_ends(symbol, "/USDT")) %>% 
      rename(usdt_pair = symbol, usdt_amount = mid_price) %>% 
      select(usdt_pair, timestamp_round, usdt_amount),
    by = c("usdt_pair", "timestamp_round")
  ) %>% 
  mutate(
    usdt_amount = if_else(usdt_pair != "USDT/USDT", usdt_amount, 1.0, NA_real_)
  ) %>% 
  
  ## calculate delta in USDT
  mutate(
    w_delta_total_refined_usdt = w_delta_total_refined * usdt_amount 
  )


arbitrage_cases %>% 
  filter(
    w_delta_total_refined_usdt > job$config$threshold_in_usdt
  ) %>% 
  arrange(timestamp_round)


## Estimate arbitrage volume

arbitrage_cases_plot <- arbitrage_cases %>% 
  mutate(
    timestamp_h = floor_date(timestamp, "hours")
  ) %>% 
  group_by(symbol, timestamp_h) %>% 
  summarise(total = sum(w_delta_total_refined)) %>% 
  
  ggplot() +
    geom_point(
      aes(y = total, x = timestamp_h, size = total, color = symbol),
      alpha = .4
    ) +
    facet_grid(symbol ~ ., scales = "free") +
    theme_bw()

png(
  sprintf("output/arbitrage_cases_plot%s.png", Sys.Date()),
  width = 1920, height = 100*length(levels(books$symbol)), units = "px"
)
print(arbitrage_cases_plot)
dev.off()

