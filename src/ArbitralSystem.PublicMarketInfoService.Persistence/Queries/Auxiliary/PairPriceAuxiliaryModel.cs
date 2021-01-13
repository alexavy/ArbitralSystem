
using System;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Queries.Auxiliary
{
    public class PairPriceAuxiliaryModel : IPairPrice
    {
        public string ExchangePairName { get; set; }
        public string UnificatedPairName { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal? Price { get; set; }
        public Exchange Exchange { get; set; }
    }
}