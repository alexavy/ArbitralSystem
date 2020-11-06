using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.PublicMarketInfoService.Services;
using Hangfire.Annotations;

namespace ArbitralSystem.PublicMarketInfoService.Workflow
{
    [UsedImplicitly]
    internal class PairInfoUpdaterJob
    {
        private readonly ILogger _logger;
        private readonly PairInfoUpdaterService _pairInfoUpdaterService;

        public PairInfoUpdaterJob(ILogger logger, PairInfoUpdaterService pairInfoUpdaterService)
        {
            Preconditions.CheckNotNull(pairInfoUpdaterService, logger);
            _logger = logger;
            _pairInfoUpdaterService = pairInfoUpdaterService;
        }

        public async Task Execute()
        {
            _logger.Information("Pair info update job started.");
            var token = new CancellationTokenSource(new TimeSpan(0, 15, 0));
            await _pairInfoUpdaterService.Update(token.Token);
        }
    }
}