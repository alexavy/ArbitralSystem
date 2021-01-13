using System.Collections.Generic;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels
{
    public interface IPairInfoPolygon
    {
        public Exchange Exchange { get; }
        public IEnumerable<IPairInfo> PolygonPairs { get; }
    }
}