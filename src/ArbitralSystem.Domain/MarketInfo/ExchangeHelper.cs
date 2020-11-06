using System;
using System.Collections.Generic;

namespace ArbitralSystem.Domain.MarketInfo
{
    public static class ExchangeHelper
    {
        public static IEnumerable<Exchange> GetAll()
        {
            var allExchanges = new List<Exchange>();
            foreach (Exchange exchange in Enum.GetValues(typeof(Exchange)))
            {
                if (exchange == Exchange.Undefined)
                    continue;

                allExchanges.Add(exchange);
            }
            return allExchanges;
        }
        
        public static Exchange ThrowIfUndefined(this Exchange exchange)
        {
            if (exchange == Exchange.Undefined)
                throw new ArgumentException("Exchange can not be undefined");
            
            return exchange;
        }
    }
}