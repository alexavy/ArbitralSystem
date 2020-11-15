
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
  
  library(ggplot2)
})

source("core-experiment.R")
theme_set(theme_bw())



# Get configs and set params ----

job <- list(
  config = config::get(),
  secrets = config::get(file = "secrets.yml")
)



# Load dataset ----

## Open DB connection
con <- dbConnect(odbc::odbc(), .connection_string = job$secrets$db_conn_string)


## Get history
orderbook_timestamps_dt <- dbGetQuery(con, "select * from orderbook_timestamps_vw")
orderbook_timestamps_dt %<>% normalize_col_names


orderbook_timestamps_dt %>% 
  select(symbol, ends_with("_timestamp")) %>% 
  pivot_longer(cols = ends_with("_timestamp")) %>% 
  
  ggplot(aes(x = value, y = symbol)) +
    geom_line()


## Get orderbook for specific pair
orderbook_dt <- dbGetQuery(con, "exec get_orderbook_sp @symbol = 'ETH/BTC', @fromDate = '2020-10-30', @toDate = '2020-10-31'")
orderbook_dt %<>% normalize_col_names


orderbook_dt %<>% 
  mutate_at(
    c("exchange", "direction", "symbol"), 
    as.factor
  ) %>% 
  mutate(
    timestamp_round = floor_date(timestamp, "second")
  )


orderbook_dt %>% 
  mutate(hour_of_day = hour(timestamp_round)) %>% 
  count(exchange, symbol, hour_of_day) %>% 
  spread(exchange, n, fill = 0) %>% 
  arrange(hour_of_day)



## Get USDT exchange rate
# TODO



## Get bot statuses
bot_statuses_dt <- dbGetQuery(con, "exec get_bot_statuses_history_sp @symbol = 'ETH/BTC', @fromDate = '2020-10-30', @toDate = '2020-10-31'")
bot_statuses_dt %<>% normalize_col_names



# Work ----

## Calculate candles

candles_dt <- orderbook_dt %>% 
  group_by(symbol, direction, timestamp_round) %>% 
  calc_candle

stopifnot(
  nrow(candles_dt) > 0,
  candles_dt %>% group_by(symbol, timestamp_round) %>% filter(n() > 1) %>% nrow == 0
)

candles_dt


## Find arbitrage opportunity

arbitrage_cases <- orderbook_dt %>% 
  
  ## get only quotes w/ arbitrage opportunity
  inner_join(
    candles_dt %>% filter(arbitrage_flag),
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
    candles_dt %>% 
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

