using System.Collections.Generic;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.PublicMarketInfoService.v1.Models
{
    public class PairInfoPolygon
    {
        public string Exchange { get; set; }
        public IEnumerable<string> UnificatedPairs { get; set; }
    }
}