using System;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Models;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands
{
    public class UpdateOrderBookDistributorStatusCommand : IRequest
    {
        public Guid DistributorId { get; }
        public DistributorStatus Status { get; }
        public Guid ServerId { get; }
        
        public UpdateOrderBookDistributorStatusCommand(Guid distributorId, DistributorStatus status, Guid serverId)
        {
            DistributorId = distributorId;
            Status = status;
            ServerId = serverId;
        }
    }
}