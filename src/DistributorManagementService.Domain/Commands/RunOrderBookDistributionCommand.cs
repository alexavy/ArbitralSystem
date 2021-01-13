using DistributorManagementService.Domain.Models;
using MediatR;

namespace DistributorManagementService.Domain.Commands
{
    public class RunOrderBookDistributionCommand : IRequest<IOrderBookDistributor>
    {
        public string UnificatedPair { get; }

        public RunOrderBookDistributionCommand(string unificatedPair)
        {
            UnificatedPair = unificatedPair;
        }
    }
}