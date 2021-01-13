using System;
using System.Linq;
using System.Linq.Expressions;
using ArbitralSystem.Common.Helpers;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Persistence.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Queries.Specs
{
    public static class PairInfoSpec
    {
        public static Expression<Func<PairInfo, bool>> ByIsActive([CanBeNull] bool? isActive)
        {
            if (!isActive.HasValue) return x => true;

            return rep => isActive.Value ? rep.UtcDelistedAt == null : rep.UtcDelistedAt != null;
        }

        public static Expression<Func<PairInfo, bool>> ByExchangePairName([CanBeNull] string exchangePairName)
        {
            if (string.IsNullOrWhiteSpace(exchangePairName)) return x => true;

            return rep => rep.ExchangePairName.ToLower().Equals(exchangePairName.ToLower());
        }

        public static Expression<Func<PairInfo, bool>> ByUnificatedPairName([CanBeNull] string unificatedPairName)
        {
            if (string.IsNullOrWhiteSpace(unificatedPairName)) return x => true;

            return rep => rep.UnificatedPairName.ToLower().Equals(unificatedPairName.ToLower());
        }
        
        public static Expression<Func<PairInfo, bool>> ByBaseCurrency([CanBeNull] string baseCurrency)
        {
            if (string.IsNullOrWhiteSpace(baseCurrency)) return x => true;

            return rep => rep.BaseCurrency.ToLower().Equals(baseCurrency.ToLower());
        }

        public static Expression<Func<PairInfo, bool>> ByQuoteCurrency([CanBeNull] string quoteCurrency)
        {
            if (string.IsNullOrWhiteSpace(quoteCurrency)) return x => true;

            return rep => rep.QuoteCurrency.ToLower().Equals(quoteCurrency.ToLower());
        }

        public static Expression<Func<PairInfo, bool>> ByExchange([CanBeNull] Exchange? exchange)
        {
            if (!exchange.HasValue) return x => true;

            return rep => rep.Exchange == (exchange.Value);
        }

        public static Expression<Func<PairInfo, bool>> ByNotExchange([CanBeNull] Exchange? exchange)
        {
            if (!exchange.HasValue) return x => true;

            return rep => rep.Exchange != (exchange.Value);
        }

        public static Expression<Func<PairInfo, bool>> ByIsDelisted(bool? isDelisted)
        {
            if (!isDelisted.HasValue) return x => true;

            return rep => isDelisted.Value ? rep.UtcDelistedAt.HasValue  : !rep.UtcDelistedAt.HasValue  ;
        }


        public static Expression<Func<PairInfo, bool>> ByListedMoreThan([CanBeNull] int? listedMoreThan, Exchange? exchange,
            PublicMarketInfoBdContext dbContext)
        {
            if (!listedMoreThan.HasValue) return x => true;

            var pairsListedMoreThan = dbContext.PairInfos.AsNoTracking()
                .Where(ByNotExchange(exchange))
                .GroupBy(o => o.UnificatedPairName)
                .Where(o => o.Count() > listedMoreThan)
                .Select(o => o.Key);

            Expression<Func<PairInfo, bool>> expression = rep => pairsListedMoreThan.Contains(rep.UnificatedPairName);
            return expression.AndSoAlso(ByExchange(exchange));
        }
    }
}