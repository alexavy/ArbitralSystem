using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Queries.Auxiliary
{
    public class PairPriceSummaryInfoAuxiliaryModel : IPairPriceSummaryInfo
    {
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? AveragePrice { get; set; }
        public string ExchangePairName { get; set; }
        public string UnificatedPairName { get; set; }
        public Exchange Exchange { get; set; }
    }
}