using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Models;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using ArbitralSystem.PublicMarketInfoService.Persistence.Queries.Specs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Queries
{
    public class PairInfoQueryHandler : Query, IRequestHandler<PairInfoByExchangeQuery, IEnumerable<IPairInfo>>
    {
        public PairInfoQueryHandler(PublicMarketInfoBdContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IEnumerable<IPairInfo>> Handle(PairInfoByExchangeQuery request, CancellationToken cancellationToken)
        {
            var pairs = await DbContext.PairInfos.AsNoTracking()
                .Where(o => o.Exchange == request.Exchange)
                .Where(PairInfoSpec.ByIsActive(request.IsActive))
                .ToArrayAsync(cancellationToken);
            return Mapper.Map<IEnumerable<IPairInfo>>(pairs);
        }
    }
}