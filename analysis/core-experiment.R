
#'
#' Core functions related to experiment
#'



#' Normalize column names of dataframe
#' 
#' @param dt 
normalize_col_names <- function(dt) {
  require(dplyr)
  stopifnot(is.data.frame(dt))
  
  names(dt) <- dt %>% names %>% tolower
  
  return(dt)
}



#' Calculate candle
#'
#' @param dt 
#'
calc_candle <- function(dt) {
  require(dplyr)
  stopifnot(is.grouped_df(dt))
  
  dt %>% 
    # calc best price
    summarise(
      price_min = min(price),
      price_max = max(price)
    ) %>% 
    mutate(
      best_price = if_else(direction == 0, price_max, price_min, NA_real_)
    ) %>% 
    select(
      -c(price_min, price_max)
    ) %>% 
    spread(direction, best_price) %>% 
    rename(
      best_ask = `1`, best_bid = `0`
    ) %>% 
    
    # calc mid price and spread
    mutate(
      mid_price = best_bid + (best_ask - best_bid)/2,
      spread = (best_ask - best_bid)/best_bid
    ) %>% 
    
    # add arbitrage_flag flag
    mutate(
      arbitrage_flag = spread < 0
    )
}

