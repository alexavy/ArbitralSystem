using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Helpers;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Commands;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Services;
using MediatR;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Domain.Handlers
{
    public class BulkSaveOrderBooksCommandHandler : IRequestHandler<BulkSaveOrderBooksCommand>
    {
        private readonly OrderBookDomainService _orderBookDomainService;
        private readonly ILogger _logger;
        
        public BulkSaveOrderBooksCommandHandler(OrderBookDomainService orderBookDomainService, ILogger logger)
        {
            Preconditions.CheckNotNull(logger);
            _orderBookDomainService = orderBookDomainService;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(BulkSaveOrderBooksCommand request, CancellationToken cancellationToken)
        {
            var result = await ArbitralStopWatch
                .MeasureInMls(async () => await _orderBookDomainService.BulkSave(request.OrderBooks.ToArray(), cancellationToken));
            _logger.Debug($"Elapsed time for saving orderbooks {result} mls");
            return Unit.Value;
        }
    }
}