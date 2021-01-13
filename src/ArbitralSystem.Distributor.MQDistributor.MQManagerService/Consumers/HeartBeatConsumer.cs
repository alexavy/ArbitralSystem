using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;
using ArbitralSystem.Messaging.Messages;
using AutoMapper;
using JetBrains.Annotations;
using MassTransit;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.Consumers
{
    /// <summary>
    /// Listening for heartbeats 
    /// </summary>
    [UsedImplicitly]
    public class HeartBeatConsumer : IConsumer<IHeartBeatOrderBookDistributorMessage>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;


        /// <summary />
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public HeartBeatConsumer(IMediator mediator, IMapper mapper, ILogger logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }
        
        /// <summary />
        public async Task Consume(ConsumeContext<IHeartBeatOrderBookDistributorMessage> context)
        {
            var heartBeats = context.Message;
            var timer = new Stopwatch();
            timer.Start();
            _logger.Debug($"HeartBeat received: {heartBeats.HeartBeatBatch.Count()} entities");
            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            try
            {
                await _mediator.Send(new OrderBookDistributorHeartBeatCommand(_mapper.Map<IEnumerable<DistributorHeartBeat>>(heartBeats.HeartBeatBatch)), 
                    tokenSource.Token);
            }
            catch (Exception e)
            {
                _logger.Error(e,"Error in updating heartBeat data");
            }
            timer.Stop();
            _logger.Debug($"HeatBeat successfully saved, time for operation: {timer.ElapsedMilliseconds}");
        }
    }
}