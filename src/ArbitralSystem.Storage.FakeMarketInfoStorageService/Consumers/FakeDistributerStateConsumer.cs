using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Messaging.Messages;
using JetBrains.Annotations;
using MassTransit;

namespace ArbitralSystem.Storage.FakeMarketInfoStorageService.Consumers
{
    [UsedImplicitly]
    public class FakeDistributerStateConsumer : IConsumer<IDistributerStateMessage>
    {
        private readonly ILogger _logger;

        public FakeDistributerStateConsumer(ILogger logger)
        {
            _logger = logger;
        }
        
        public Task Consume(ConsumeContext<IDistributerStateMessage> context)
        {
            _logger.Information("State message received: {@mess}",context.Message);
            return Task.CompletedTask;
        }
    }
}