using System.Collections.Generic;
using System.Linq;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Models;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Services
{
    internal static class PairInfoCompare
    {
        internal static IEnumerable<PairInfo> ExceptBy(this IEnumerable<PairInfo> @this, IEnumerable<PairInfo> @from, Exchange exchange)
        {
            return @this.Where(ex => !@from
                .Any(cur => cur.ExchangePairName.Equals(ex.ExchangePairName) && 
                            cur.Exchange.Equals(ex.Exchange)));
        }

    }
}