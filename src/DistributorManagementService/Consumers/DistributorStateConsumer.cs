using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Domain.Distributers;
using ArbitralSystem.Messaging.Messages;
using DistributorManagementService.Domain.Commands;
using DistributorManagementService.Domain.Exceptions;
using MassTransit;

namespace DistributorManagementService.Consumers
{
    public class DistributorStateConsumer : IConsumer<IDistributorMessage>
    {
        private readonly MediatR.IMediator _mediator;
        private readonly ILogger _logger;
        
        public DistributorStateConsumer(MediatR.IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<IDistributorMessage> context)
        {
            var distributorState = context.Message;
            switch (distributorState.DistributorState)
            {
                case DistributorState.Initialization:
                    await CreateDistributor(context.Message);
                    break;
                case DistributorState.Deleted:
                    await DeleteDistributor(context.Message);
                    break;
                case DistributorState.Listening:
                case DistributorState.Distribution:
                case DistributorState.Stopping:
                    await UpdateStateOfDistributor(context.Message);
                    break;
                default:
                    _logger.Error("UnSupported state received {@message}",distributorState);
                    throw new InvalidDistributorOperation($"UnSupported state received: {distributorState.DistributorState}");
            }
        }

        private async Task CreateDistributor(IDistributorMessage distributorMessage)
        {
            var command = new BotCreatedCommand(distributorMessage.DistributorId, distributorMessage.DistributorName);
            var distributor = await _mediator.Send(command);
        }
        
        private async Task DeleteDistributor(IDistributorMessage distributorMessage)
        {
            var command = new BotDeleteCommand(distributorMessage.DistributorId);
            var distributor = await _mediator.Send(command);
        }

        private async Task UpdateStateOfDistributor(IDistributorMessage distributorMessage)
        {
            var command = new UpdateOrderBookDistributionStateCommand(distributorMessage.DistributorId, distributorMessage.DistributorState);
            var distributor = await _mediator.Send(command);
        }
        
    }
}