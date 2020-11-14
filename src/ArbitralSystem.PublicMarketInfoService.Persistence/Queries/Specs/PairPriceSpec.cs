using System;
using System.Linq.Expressions;
using ArbitralSystem.PublicMarketInfoService.Persistence.Entities;
using JetBrains.Annotations;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Queries.Specs
{
    public class PairPriceSpec
    {
        public static Expression<Func<PairPrice, bool>> ByFrom([CanBeNull] DateTimeOffset? from)
        {
            if (!from.HasValue) return x => true;

            return rep => rep.Date >= from;
        }
        
        public static Expression<Func<PairPrice, bool>> ByTo([CanBeNull] DateTimeOffset? to)
        {
            if (!to.HasValue) return x => true;

            return rep => rep.Date <= to;
        }
    }
}