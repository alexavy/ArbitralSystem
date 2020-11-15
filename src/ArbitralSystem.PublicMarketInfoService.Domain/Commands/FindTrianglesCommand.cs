using System.Collections.Generic;
using System.Linq;
using ArbitralSystem.PublicMarketInfoService.Domain.Models;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Commands
{
    public class FindTrianglesCommand : IRequest<IEnumerable<IPairInfoPolygon>>
    {
        public IPairInfo[] PairInfos { get; }
        public PolygonFilter Filter { get; }

        public FindTrianglesCommand(IEnumerable<IPairInfo> pairInfos, PolygonFilter filter = null)
        {
            PairInfos = pairInfos.ToArray();
            Filter = filter;
        }
    }
}