using ArbitralSystem.Common.Pagination;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters
{
    public class UniquePairInfoFilterQuery : IRequest<Page<IUniquePairInfo>>
    {
        public PairInfoFilter Filter { get; }
        public UniquePairInfoFilterQuery(PairInfoFilter filter)
        {
            Filter = filter;
        }
    }
}