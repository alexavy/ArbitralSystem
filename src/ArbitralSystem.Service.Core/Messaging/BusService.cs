using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace ArbitralSystem.Service.Core.Messaging
{
    public class BusService : IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly ILogger _logger; 
        
        public BusService(IBusControl busControl,
            ILogger logger)
        {
            _logger = logger;
            _busControl = busControl;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync(cancellationToken);
            _logger.Debug("Bus message service started.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync(cancellationToken);
            _logger.Debug("Bus message service stopped.");
        }
    }
}