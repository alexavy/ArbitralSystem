using System;
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
    public class SaveDistributerStateCommandHandler : IRequestHandler<SaveDistributerStateCommand, Guid>
    {
        private readonly DistributerStateDomainService _distributerStateDomainService;
        private readonly ILogger _logger;

        public SaveDistributerStateCommandHandler(ILogger logger, DistributerStateDomainService distributerStateDomainService)
        {
            Preconditions.CheckNotNull(logger, distributerStateDomainService);
            _distributerStateDomainService = distributerStateDomainService;
            _logger = logger;
        }

        public async Task<Guid> Handle(SaveDistributerStateCommand request, CancellationToken cancellationToken)
        {
            var result = await ArbitralStopWatch
                .MeasureInMls(async () => await _distributerStateDomainService.SaveState(request.State, cancellationToken));
            _logger.Debug($"Elapsed time for saving distributor state {request.State.Id} - {result.ElapsedInMls} mls");
            return result.Result.Id;
        }
    }
}