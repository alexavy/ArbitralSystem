using System;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Models;
using ArbitralSystem.Messaging.Messages;
using AutoMapper;
using JetBrains.Annotations;
using MassTransit;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.Consumers
{
    /// <summary>
    /// Consumer which listen for activating and di activating servers
    /// </summary>
    [UsedImplicitly]
    public class ServerConsumer : IConsumer<IServerCreatedMessage>, IConsumer<IServerDeletedMessage>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        /// <summary />
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public ServerConsumer(IMediator mediator, IMapper mapper, ILogger logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<IServerCreatedMessage> context)
        {
            _logger.Information("Server created message received, message: {@mes}", context.Message);
            var command = new CreateServerCommand(_mapper.Map<Server>(context.Message));
            await _mediator.Send(command);
        }

        /// <inheritdoc />
        public async Task Consume(ConsumeContext<IServerDeletedMessage> context)
        {
            _logger.Information($"Server deleted message received, server id: {context.Message.ServerId}");
            await _mediator.Send(new DeleteServerCommand(context.Message.ServerId));
        }
    }
}