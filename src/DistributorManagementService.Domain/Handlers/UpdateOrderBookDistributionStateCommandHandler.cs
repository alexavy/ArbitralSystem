using System.Threading;
using System.Threading.Tasks;
using DistributorManagementService.Domain.Commands;
using DistributorManagementService.Domain.Models;
using DistributorManagementService.Domain.Services;
using MediatR;

namespace DistributorManagementService.Domain.Handlers
{
    public class UpdateOrderBookDistributionStateCommandHandler : IRequestHandler<UpdateOrderBookDistributionStateCommand, IOrderBookDistributor>
    {
        private readonly OrderBookDistributorDomainService _domainService;

        public UpdateOrderBookDistributionStateCommandHandler(OrderBookDistributorDomainService domainService)
        {
            _domainService = domainService;
        }
        
        public async Task<IOrderBookDistributor> Handle(UpdateOrderBookDistributionStateCommand request, CancellationToken cancellationToken)
        {
            return await _domainService.UpdateStateAsync(request.Id, request.State, cancellationToken);
        }
    }
}