using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.PublicMarketInfoService.Domain.Commands;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using ArbitralSystem.PublicMarketInfoService.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Handlers
{
    public class FindChainsCommandHandler : IRequestHandler<FindTrianglesCommand, IEnumerable<IPairInfoPolygon>>
    {
        private readonly PairInfoChainService _chainService;

        public FindChainsCommandHandler(PairInfoChainService chainService)
        {
            Preconditions.CheckNotNull(chainService);
            _chainService = chainService;
        }

        public Task<IEnumerable<IPairInfoPolygon>> Handle(FindTrianglesCommand request, CancellationToken cancellationToken)
        {
            var filteredPolygons = _chainService.FindTriangles(request.PairInfos)
                .Where(o => request.Filter?.BaseCurrency == null || o.PolygonPairs.Any(x => x.BaseCurrency == request.Filter?.BaseCurrency)
                            && request.Filter?.QuoteCurrency == null || o.PolygonPairs.Any(x => x.QuoteCurrency == request.Filter?.QuoteCurrency)
                            && request.Filter?.UnificatedPairName == null ||
                            o.PolygonPairs.Any(x => x.UnificatedPairName == request.Filter?.UnificatedPairName))
                .Select(o => o as IPairInfoPolygon);

            return Task.FromResult(filteredPolygons);
        }
    }
}