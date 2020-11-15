using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributor.Core.Models
{
    public class PairInfo
    {
        public Exchange Exchange { get ; }
        public string UnificatedPairName { get; }
        public string ExchangePairName { get; }

        public PairInfo(Exchange exchange, string unificatedPairName, string exchangePairName)
        {
            if(exchange == Exchange.Undefined)
                throw new ArgumentException("Exchange can not be undefined.");
            Exchange = exchange;

            if(string.IsNullOrEmpty(unificatedPairName) || string.IsNullOrEmpty(exchangePairName))
                throw new ArgumentException("pair can not be empty or null");

            UnificatedPairName = unificatedPairName;
            ExchangePairName = exchangePairName;
        }
    }
}