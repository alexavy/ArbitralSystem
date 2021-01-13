using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries
{
    public class SummaryPairPriceQuery : IRequest<IPairPriceSummaryInfo>
    {
        public IntervalFilter PageFilter { get;  }
        public string UnificatedPairName { get; }
        public Exchange Exchange { get; }

        public SummaryPairPriceQuery(string unificatedPairName, Exchange exchange, IntervalFilter pageFilter)
        {
            UnificatedPairName = unificatedPairName;
            Exchange = exchange;
            PageFilter = pageFilter;
        }
    }
}