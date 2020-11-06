using System;
using System.Collections.Generic;
using ArbitralSystem.Messaging.Models;

namespace ArbitralSystem.Messaging.Messages
{
    public interface IHeartBeatOrderBookDistributorMessage : ICorrelation
    {
        public IEnumerable<HeartBeatOrderBookDistributor> HeartBeatBatch { get; }
    }
}