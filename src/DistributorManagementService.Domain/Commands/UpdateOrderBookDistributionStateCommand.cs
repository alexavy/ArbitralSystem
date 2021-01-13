using System;
using ArbitralSystem.Domain.Distributers;
using DistributorManagementService.Domain.Models;
using MediatR;

namespace DistributorManagementService.Domain.Commands
{
    public class UpdateOrderBookDistributionStateCommand : IRequest<IOrderBookDistributor>
    {
        public Guid Id { get; }
        public DistributorState State { get; }

        public UpdateOrderBookDistributionStateCommand(Guid id,DistributorState state)
        {
            Id = id;
            State = state;
        }
    }
}