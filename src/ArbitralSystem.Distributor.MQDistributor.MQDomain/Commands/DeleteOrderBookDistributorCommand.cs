using System;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands
{
    public class DeleteOrderBookDistributorCommand : IRequest
    {
        public Guid Id { get; }

        public DeleteOrderBookDistributorCommand(Guid id)
        {
            Id = id;
        }
    }
}