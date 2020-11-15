using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries
{
    public class PairPriceQuery : IRequest<Page<IPairPrice>>
    {
        public IntervalPageFilter PageFilter { get;  }
        public string UnificatedPairName { get; }
        public Exchange Exchange { get; }

        public PairPriceQuery(string unificatedPairName, Exchange exchange, IntervalPageFilter pageFilter)
        {
            UnificatedPairName = unificatedPairName;
            Exchange = exchange;
            PageFilter = pageFilter;
        }
    }
}