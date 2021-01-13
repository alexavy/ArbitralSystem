using System;
using DistributorManagementService.Domain.Models;
using MediatR;

namespace DistributorManagementService.Domain.Commands
{
    public class StopOrderBookDistributionCommand : IRequest
    {
        public Guid Id { get; }

        public StopOrderBookDistributionCommand(Guid id)
        {
            Id = id;
        }
    }
}