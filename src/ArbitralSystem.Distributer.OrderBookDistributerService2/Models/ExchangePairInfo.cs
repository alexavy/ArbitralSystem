using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributer.OrderBookDistributerService.Models
{
    public class ExchangePairInfo
    {
        public string UnificatedPairName { get; }
        public IEnumerable<PairInfo> PairInfos { get; }

        public ExchangePairInfo(PairInfo[] pairInfos)
        {
            if(!pairInfos.Any())
                throw new ArgumentException("Pairs should not be empty");
            
            if(pairInfos.GroupBy(o => o.UnificatedPairName).Any(o => o.Count() != pairInfos.Count()))
                throw new ArgumentException("Not all pairs have unique name");

            PairInfos = pairInfos;
            UnificatedPairName = pairInfos.First().UnificatedPairName;
        }
    }

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