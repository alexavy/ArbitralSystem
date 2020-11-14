using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Models
{
    public class PairPrice
    {
        public string ExchangePairName { get; }
        public decimal? Price { get; }
        public Exchange Exchange { get; }
        public DateTimeOffset CreatedAt { get; set; }

        public PairPrice(string exchangePairName, decimal? price, Exchange exchange)
        {
            if(string.IsNullOrEmpty(exchangePairName))
                throw new ArgumentException($"{nameof(exchangePairName)} Can't be empty or null in pair price info");
            ExchangePairName = exchangePairName;
            Price = price;
            
            if(exchange.Equals(Exchange.Undefined))
                throw new ArgumentException($"{nameof(exchange)} Can't be undefined");
            Exchange = exchange;
            CreatedAt = DateTimeOffset.Now;
        }
    }
}