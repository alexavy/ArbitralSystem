using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Storage.MarketInfoStorageService.Persistence;
using JetBrains.Annotations;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ArbitralSystem.Storage.MarketInfoStorageService
{
    [UsedImplicitly]
    internal class StorageService : IHostedService, IDisposable
    {
        private readonly MarketInfoBdContext _ctx;
        private readonly IBusControl _busControl;
        private readonly ILogger _logger;

        public StorageService(MarketInfoBdContext ctx, IBusControl busControl,
            ILogger logger)
        {
            _ctx = ctx;
            _busControl = busControl;
            _logger = logger;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await MigrateDb();
            _logger.Information($"{nameof(StorageService)} started.");
            await _busControl.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information($"{nameof(StorageService)} stopped.");
            await _busControl.StopAsync(cancellationToken);
        }
        
        public void Dispose()
        {
            _logger.Information($"{nameof(StorageService)} disposed.");
        }

        private async Task MigrateDb()
        {
            await _ctx.Database.MigrateAsync();
            await _ctx.SaveChangesAsync();
        }
        
    }
}