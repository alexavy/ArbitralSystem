using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands;
using ArbitralSystem.Messaging.Messages;
using AutoMapper;
using JetBrains.Annotations;
using MassTransit;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.Consumers
{
    /// <summary>
    /// Listening for changing statuses of distributors. 
    /// </summary>
    [UsedImplicitly]
    public class OrderBookDistributorConsumer : IConsumer<IOrderBookDistributorStatusMessage>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;


        /// <summary />
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public OrderBookDistributorConsumer(IMediator mediator, IMapper mapper, ILogger logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }
        
        /// <summary />
        public async Task Consume(ConsumeContext<IOrderBookDistributorStatusMessage> context)
        {
            var distributorStatus = context.Message;
            await _mediator.Send(new UpdateOrderBookDistributorStatusCommand(distributorStatus.DistributorId,
                _mapper.Map<MQDomain.Common.DistributorStatus>(distributorStatus.DistributorStatus), distributorStatus.ServerId));
        }
    }
}