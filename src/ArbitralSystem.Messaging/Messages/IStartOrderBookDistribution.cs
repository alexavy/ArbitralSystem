using System;
using System.Collections.Generic;
using ArbitralSystem.Messaging.Models;

namespace ArbitralSystem.Messaging.Messages
{
    public interface IStartOrderBookDistribution : ICorrelation
    {
        public Guid DistributorId { get; }
        public string UnificatedPairName { get; }
        public IEnumerable<PairInfo> PairInfos { get; }
    }

    public interface IStopOrderBookDistribution : ICorrelation
    {
        public Guid DistributorId { get; }
    }
}