using System;
using System.Collections.Generic;
using System.Linq;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using Microsoft.EntityFrameworkCore.Internal;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Models
{
    public class PairInfoPolygon : IPairInfoPolygon
    {
        public PairInfoPolygon(IPairInfo[] polygonPairs)
        {
            if(!polygonPairs.Any())
                throw new ArgumentException("Polygon should not have no corners.");

            Exchange = polygonPairs.First().Exchange;
            
            if(polygonPairs.Any(o=>o.Exchange != Exchange))
                throw new ArgumentException("Pairs should be from one exchange.");
            
            PolygonPairs = polygonPairs;
            CornersNumber = polygonPairs.Count();
        }
        
        public Exchange Exchange { get; }
        public IEnumerable<IPairInfo> PolygonPairs { get; }
        public int CornersNumber { get; }
        public string PairsPolygonIdentity
        {
            get
            {
                if (PolygonPairs.Any())
                {
                    return PolygonPairs.OrderBy(o => o.UnificatedPairName)
                        .Select(k => k.UnificatedPairName)
                        .Aggregate((c, n) => c + ';' + n);
                }
                else
                    return null;
            }
        }
    }
}