using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.PublicMarketInfoService.Domain.Commands;
using ArbitralSystem.PublicMarketInfoService.Domain.Services;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Handlers
{
    public class PairPricesCommandHandler :  IRequestHandler<SaveLastPairPricesCommand>
    {
        private readonly PairPricesDomainService _pricesDomainService;

        public PairPricesCommandHandler(PairPricesDomainService pricesDomainService)
        {
            _pricesDomainService = pricesDomainService;
        }

        public async Task<Unit> Handle(SaveLastPairPricesCommand request, CancellationToken cancellationToken)
        {
            await _pricesDomainService.SaveLastPrices(request.Exchanges, cancellationToken);
            return Unit.Value;
        }
    }
}