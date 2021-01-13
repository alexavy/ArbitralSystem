using System.Threading;
using System.Threading.Tasks;
using DistributorManagementService.Domain.Commands;
using DistributorManagementService.Domain.Services;
using MediatR;

namespace DistributorManagementService.Domain.Handlers
{
    public class StopOrderBookDistributionCommandHandler : IRequestHandler<StopOrderBookDistributionCommand>
    {
        private readonly OrderBookDistributorDomainService _domainService;

        public StopOrderBookDistributionCommandHandler(OrderBookDistributorDomainService domainService)
        {
            _domainService = domainService;
        }

        public async Task<Unit> Handle(StopOrderBookDistributionCommand request, CancellationToken cancellationToken)
        {
            await _domainService.StopBotAsync(request.Id, cancellationToken);
            return Unit.Value;
        }
    }
}