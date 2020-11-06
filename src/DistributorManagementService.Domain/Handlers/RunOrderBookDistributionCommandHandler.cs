using System.Threading;
using System.Threading.Tasks;
using DistributorManagementService.Domain.Commands;
using DistributorManagementService.Domain.Models;
using DistributorManagementService.Domain.Services;
using MediatR;

namespace DistributorManagementService.Domain.Handlers
{
    public class RunOrderBookDistributionCommandHandler : IRequestHandler<RunOrderBookDistributionCommand,IOrderBookDistributor>
    {
        private readonly OrderBookDistributorDomainService _domainService;

        public RunOrderBookDistributionCommandHandler(OrderBookDistributorDomainService domainService)
        {
            _domainService = domainService;
        }
        
        public async Task<IOrderBookDistributor> Handle(RunOrderBookDistributionCommand request, CancellationToken cancellationToken)
        {
            return await _domainService.RunBotAsync(request.UnificatedPair, cancellationToken);
        }
    }
    

}