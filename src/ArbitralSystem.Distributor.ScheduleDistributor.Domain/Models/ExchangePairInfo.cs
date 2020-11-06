using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models
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

            if(pairInfos.GroupBy(o => o.Exchange).Any(o => o.Count() != 1))
                throw new ArgumentException("Not all pairs have same exchange");
            
            PairInfos = pairInfos;
            UnificatedPairName = pairInfos.First().UnificatedPairName;
        }
    }

    public class DistributorExchangePairs 
    {
        public string QueueName { get; }
        public string ServerName { get; }
        public ExchangePairInfo ExchangePairInfo { get; }
        
        public DistributorExchangePairs(string queueName, string serverName, ExchangePairInfo exchangePairInfo) 
        {
            if(string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(serverName))
                throw new ArgumentNullException($"Server name or queue can't bu null");

            QueueName = queueName;
            ServerName = serverName;
            ExchangePairInfo = exchangePairInfo ?? throw new ArgumentNullException($"Exchange pair info can't bu null");
        }
    }
}