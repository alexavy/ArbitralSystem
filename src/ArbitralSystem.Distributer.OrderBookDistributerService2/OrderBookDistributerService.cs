using ArbitralSystem.Common.Logger;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Distributer.OrderBookDistributerService.Workflow;
using JetBrains.Annotations;

namespace ArbitralSystem.Distributer.OrderBookDistributerService
{
    [UsedImplicitly]
    internal class OrderBookDistributerService : IHostedService
    {
        private readonly IOrderBookDistributerWorkflow _distributerWorkflow;
        private readonly ILogger _logger;

        public OrderBookDistributerService(IOrderBookDistributerWorkflow distributerWorkflow,
            ILogger logger)
        {
            _distributerWorkflow = distributerWorkflow;
            _logger = logger;
        }
        
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Information("OrderBookDistributerService started.");
            await _distributerWorkflow.StartListeningForPairs(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information("OrderBookDistributerService stopped.");
            return Task.CompletedTask;
        }
    }
}