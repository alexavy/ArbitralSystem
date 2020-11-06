using System;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Models;

namespace ArbitralSystem.Distributor.MQDistributor.MQOrderBookDistributorService.Messaging
{
    internal class OrderBookDistributorStatusMessage : BaseMessage, IOrderBookDistributorStatusMessage
    {
        public OrderBookDistributorStatusMessage(Guid distributorId, DistributorStatus distributorStatus, Guid serverId)
        {
            DistributorId = distributorId;
            DistributorStatus = distributorStatus;
            ServerId = serverId;
        }
        
        public Guid DistributorId { get; }
        public DistributorStatus DistributorStatus { get; }
        public Guid ServerId { get; }
    }
}