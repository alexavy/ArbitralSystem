using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels
{
    public interface IPairPriceSummaryInfo 
    {
        decimal? MaxPrice { get; }
        decimal? MinPrice { get; }
        decimal? AveragePrice { get; }
        string ExchangePairName { get; }
        string UnificatedPairName { get; }
        Exchange Exchange { get; }
    }
}