using System.Threading;
using System.Threading.Tasks;
using DistributorManagementService.Domain.Commands;
using DistributorManagementService.Domain.Models;
using DistributorManagementService.Domain.Services;
using MediatR;

namespace DistributorManagementService.Domain.Handlers
{
    public class BotCreatedCommandHandler : IRequestHandler<BotCreatedCommand,IDistributor>
    {
        private readonly OrderBookDistributorDomainService _domainService;

        public BotCreatedCommandHandler(OrderBookDistributorDomainService domainService)
        {
            _domainService = domainService;
        }
        
        public async Task<IDistributor> Handle(BotCreatedCommand request, CancellationToken cancellationToken)
        {
            return await _domainService.CreateBotAsync(request.Id, request.Name, cancellationToken);
        }
    }
}