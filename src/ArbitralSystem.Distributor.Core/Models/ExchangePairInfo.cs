using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitralSystem.Distributor.Core.Models
{
    public class ExchangePairInfo
    {
        public string UnificatedPairName { get; }
        public IEnumerable<PairInfo> PairInfos { get; }

        public ExchangePairInfo(IEnumerable<PairInfo> pairInfos)
        {
            if(!pairInfos.Any())
                throw new ArgumentException("Pairs should not be empty");
            
            if(pairInfos.GroupBy(o => o.UnificatedPairName).Any(o => o.Count() != pairInfos.Count()))
                throw new ArgumentException("Not all pairs have unique name");

            if(pairInfos.GroupBy(o => o.Exchange).Any(o => o.Count() != 1))
                throw new ArgumentException("Not all pairs have same exchange");
            
            PairInfos = pairInfos;
            UnificatedPairName = pairInfos.First().UnificatedPairName;
        }
    }
}