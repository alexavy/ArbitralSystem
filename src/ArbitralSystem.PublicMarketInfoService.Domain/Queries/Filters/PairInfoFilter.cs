using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Domain.MarketInfo;
using JetBrains.Annotations;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters
{
    [UsedImplicitly]
    public class PairInfoFilter : PageFilter
    {
        public string UnificatedPairName { get; set; }
        public string ExchangePairName { get; set; }
        public string BaseCurrency { get; set;}
        public string QuoteCurrency { get; set;}
        public Exchange? Exchange { get; set; }
        public bool? IsDelisted { get; set; } 
        public int? ListedMoreThan { get; set; }
    }
}