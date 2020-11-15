using System;
using ArbitralSystem.Messaging.Messages;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Common.Messaging
{
    internal class StopOrderBookDistribution : BaseMessage , IStopOrderBookDistribution
    {
        public Guid DistributorId { get; }

        public StopOrderBookDistribution(Guid distributorId)
        {
            DistributorId = distributorId;
        }
    }
}