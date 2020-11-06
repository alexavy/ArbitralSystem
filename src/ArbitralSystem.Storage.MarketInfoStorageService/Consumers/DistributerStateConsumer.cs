using System;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Commands;
using JetBrains.Annotations;
using MassTransit;
using MediatR;
using DistributerState = ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models.DistributerState;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Consumers
{
    [UsedImplicitly]
    internal class DistributerStateConsumer : IConsumer<IDistributerStateMessage>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;


        public DistributerStateConsumer(IMediator mediator, ILogger logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<IDistributerStateMessage> context)
        {
            _logger.Information("Distributor message state received: {@mess}, retry attempt: {attempt}", context.Message, context.GetRetryAttempt());

            try
            {
                var state = new DistributerState(Guid.NewGuid(), context.Message.Symbol, context.Message.Exchange, context.Message.ChangedAt,
                    context.Message.PreviousStatus, context.Message.CurrentStatus);
                await _mediator.Send(new SaveDistributerStateCommand(state));
            }
            catch (Exception e)
            {
                _logger.Error(e,"Error while saving order book distributor state");
                throw;
            }
            
        }
    }
}