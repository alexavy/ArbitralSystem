using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels
{
    public interface IPairPrice 
    {
        string ExchangePairName { get; }
        string UnificatedPairName { get; }
        DateTimeOffset Date { get; }
        decimal? Price { get; }
        Exchange Exchange { get; }
    }
}