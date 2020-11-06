using System;
using System.Collections.Generic;
using System.Linq;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Models;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Services
{
    [UsedImplicitly]
    public class PairInfoChainService
    {
        private readonly ILogger _logger;
        public PairInfoChainService(ILogger logger)
        {
            Preconditions.CheckNotNull(logger);
            _logger = logger;
        }
        
        public IEnumerable<PairInfoPolygon> FindTriangles(IPairInfo[] exchangePairs)
        {
            ValidatePairs(exchangePairs);
            var polygons = new List<PairInfoPolygon>();
            foreach (var pair in exchangePairs)
            {
                var firstPossibleCorners = exchangePairs.Where(o => o.QuoteCurrency == pair.QuoteCurrency);

                foreach (var firstPossibleCorner in firstPossibleCorners)
                {
                    var secondCorners = exchangePairs.Where(o =>
                        o.QuoteCurrency == firstPossibleCorner.BaseCurrency && o.BaseCurrency == pair.BaseCurrency).ToArray();

                    if (secondCorners.Any())
                    {
                        if (secondCorners.Count() > 1)
                            _logger.Warning("UnNormal situation, more then one last corners in triangle");
                        
                        polygons.Add(new PairInfoPolygon(new [] {pair,firstPossibleCorner,secondCorners.First()}));
                    }
                }
            }
            return polygons.GroupBy(plg => plg.PairsPolygonIdentity).Select(o => o.First());
        }

        private void ValidatePairs(IPairInfo[] pairs)
        {
            if (pairs.Any())
            {
                var exchange = pairs.First().Exchange;
                if(pairs.Any(o=>o.Exchange != exchange))
                    throw new ArgumentException("Can't find polygons, pairs should be from one exchange");
            }
        }
    }
}