using System;
using System.Collections.Generic;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Models;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Common.Messaging
{
    internal class StartOrderBookDistribution : BaseMessage , IStartOrderBookDistribution
    {
        public StartOrderBookDistribution( Guid distributorId, string unificatedPairName, PairInfo[] pairInfos)
        {
            DistributorId = distributorId;
            UnificatedPairName = unificatedPairName;
            PairInfos = pairInfos;
        }
        public Guid DistributorId { get; }
        public string UnificatedPairName { get; }
        public IEnumerable<PairInfo> PairInfos { get; }
    }
}