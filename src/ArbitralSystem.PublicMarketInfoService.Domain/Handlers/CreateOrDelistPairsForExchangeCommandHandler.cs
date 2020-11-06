using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.PublicMarketInfoService.Domain.Commands;
using ArbitralSystem.PublicMarketInfoService.Domain.Services;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Handlers
{
    public class CreateOrDelistPairsForExchangeCommandHandler : IRequestHandler<CreateOrDelistPairsForExchangeCommand>
    {
        private readonly PairInfoDomainService _pairInfoDomainService;
        
        public CreateOrDelistPairsForExchangeCommandHandler(PairInfoDomainService pairInfoDomainService)
        {
            Preconditions.CheckNotNull(pairInfoDomainService);
            _pairInfoDomainService = pairInfoDomainService;
        }
        
        public async Task<Unit> Handle(CreateOrDelistPairsForExchangeCommand request, CancellationToken cancellationToken)
        {
            await _pairInfoDomainService.CreateOrDelistPair(request.Exchange, cancellationToken);
            return Unit.Value;
        }
    }
}