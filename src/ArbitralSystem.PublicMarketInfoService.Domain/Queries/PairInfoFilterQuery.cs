using ArbitralSystem.Common.Pagination;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries
{
    public class PairInfoFilterQuery : IRequest<Page<IPairInfo>>
    {
        public PairInfoFilter Filter { get; }
        public PairInfoFilterQuery(PairInfoFilter filter)
        {
            Filter = filter;
        }
    }
}