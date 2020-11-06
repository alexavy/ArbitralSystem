using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService.Options;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService
{
    internal class OrderBookDistributerService : IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly ILogger _logger;
        private readonly ServerOptions _serverOptions;
        private readonly IServiceProvider _serviceProvider;
        private BackgroundJobServer _server;
        public OrderBookDistributerService(IBusControl busControl,ServerOptions serverOptions, ILogger logger
        ,IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serverOptions = serverOptions;
            _busControl = busControl;
            _logger = logger;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync(cancellationToken);
            GlobalConfiguration.Configuration
                .UseActivator(new HangfireActivator(_serviceProvider));
            _server = new BackgroundJobServer(new BackgroundJobServerOptions()
            {
                     WorkerCount = 100,
                     Queues = new[] {"orderbook-queue"},
                     ServerName = _serverOptions.ServerName
            });
            _logger.Information("OrderBook distributed service started");
        }

        public async  Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information("OrderBook distributed service stopped");
            _server.Dispose();
            await Task.Delay(5000);
            await _busControl.StopAsync(cancellationToken);
        }
    }
    
    public class HangfireActivator : Hangfire.JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type type)
        {
            return _serviceProvider.GetService(type);
        }
    }  
}