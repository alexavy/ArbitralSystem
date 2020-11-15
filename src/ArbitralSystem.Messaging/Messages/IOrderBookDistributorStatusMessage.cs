using System;
using ArbitralSystem.Messaging.Models;

namespace ArbitralSystem.Messaging.Messages
{
    public interface IOrderBookDistributorStatusMessage : ICorrelation
    {
        public Guid DistributorId { get; }
        public DistributorStatus DistributorStatus { get; }
        public Guid ServerId { get; }
    }
}