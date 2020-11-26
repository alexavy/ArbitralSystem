using System.Linq;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Messaging.Messages;
using JetBrains.Annotations;
using MassTransit;

namespace ArbitralSystem.Storage.FakeMarketInfoStorageService.Consumers
{
    [UsedImplicitly]
    public class FakeOrderBookConsumer :  IConsumer<IOrderBookPackageMessage>
    {
        private readonly ILogger _logger;

        public FakeOrderBookConsumer(ILogger logger)
        {
            _logger = logger;
        }
        
        public Task Consume(ConsumeContext<IOrderBookPackageMessage> context)
        {
            _logger
                .Information($"OrderBook message received, total: {context.Message.OrderBooks.Count()}," +
                             $" Id {context.Message.CorrelationId} ");
            return Task.CompletedTask;
        }
    }
}