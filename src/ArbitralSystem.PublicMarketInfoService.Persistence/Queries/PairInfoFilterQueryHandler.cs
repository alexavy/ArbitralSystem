using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Common.Pagination.SqlServer;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using ArbitralSystem.PublicMarketInfoService.Persistence.Mapping.AuxiliaryModels;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Queries
{
    public class PairInfoFilterQueryHandler : BasePairInfoFilterQueryHandler, IRequestHandler<PairInfoFilterQuery, Page<IPairInfo>>,
        IRequestHandler<UniquePairInfoFilterQuery, Page<IUniquePairInfo>>,
        IRequestHandler<PairInfosByUnificatedNameQuery, IEnumerable<IPairInfo>>
    {
        public PairInfoFilterQueryHandler(PublicMarketInfoBdContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<Page<IPairInfo>> Handle(PairInfoFilterQuery request, CancellationToken cancellationToken)
        {
            return await FullPairInfoFilterQuery(request.Filter)
                .OrderBy(ci => ci.ExchangePairName)
                .Page(request.Filter,Mapper.Map<IPairInfo>, cancellationToken);
        }
        
        public async Task<Page<IUniquePairInfo>> Handle(UniquePairInfoFilterQuery request, CancellationToken cancellationToken)
        {
            var result = await FullPairInfoFilterQuery(request.Filter).GroupBy(o => o.UnificatedPairName)
                .Select(o => new
                {
                    UnificatedPairName = o.Key,
                    Occurencies = o.Count()
                })
                .OrderBy(o => o.UnificatedPairName)
                .Page(request.Filter, cancellationToken);
            
            return new Page<IUniquePairInfo>(result.Items.Select(o => new UniquePairInfoPreparer(o.UnificatedPairName, o.Occurencies)), result.Total);
        }
        
        public async Task<IEnumerable<IPairInfo>> Handle(PairInfosByUnificatedNameQuery request, CancellationToken cancellationToken)
        {
            var result = await DbContext.PairInfos.AsNoTracking()
                .Where(o => o.UnificatedPairName == request.UnificatedPairName)
                .ToArrayAsync(cancellationToken);

            return Mapper.Map<IEnumerable<IPairInfo>>(result);
        }
    }
}