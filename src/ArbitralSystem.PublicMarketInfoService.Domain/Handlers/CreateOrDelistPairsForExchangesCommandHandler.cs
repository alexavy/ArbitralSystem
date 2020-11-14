using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.PublicMarketInfoService.Domain.Commands;
using ArbitralSystem.PublicMarketInfoService.Domain.Services;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Handlers
{
    public class PairsInfoCommandHandler : IRequestHandler<CreateOrDelistPairsForExchangesCommand>
    {
        private readonly PairInfoDomainService _pairInfoDomainService;
        
        public PairsInfoCommandHandler(PairInfoDomainService pairInfoDomainService)
        {
            Preconditions.CheckNotNull(pairInfoDomainService);
            _pairInfoDomainService = pairInfoDomainService;
        }
        
        public async Task<Unit> Handle(CreateOrDelistPairsForExchangesCommand request, CancellationToken cancellationToken)
        {
            foreach (var exchange in request.Exchanges)
            {
                await _pairInfoDomainService.CreateOrDelistPair(exchange, cancellationToken);
            }
            return Unit.Value;
        }
    }
}