using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Distributor.Core.Jobs;
using ArbitralSystem.Distributor.Core.Models;
using ArbitralSystem.Distributor.MQDistributor.MQOrderBookDistributorService.Jobs;
using ArbitralSystem.Messaging.Messages;
using AutoMapper;
using JetBrains.Annotations;
using MassTransit;

namespace ArbitralSystem.Distributor.MQDistributor.MQOrderBookDistributorService.Consumers
{
    [UsedImplicitly]
    public class OrderBookDistributorConsumer : IConsumer<IStartOrderBookDistribution>
    {
        private readonly OrderBookDistributorJob _distributorJob;
        private readonly JobManager _jobManager;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public OrderBookDistributorConsumer(OrderBookDistributorJob distributorJob, JobManager jobManager, IMapper mapper, ILogger logger)
        {
            _mapper = mapper;
            _logger = logger;
            _jobManager = jobManager;
            _distributorJob = distributorJob;
        }

        public async Task Consume(ConsumeContext<IStartOrderBookDistribution> context)
        {
            var pairInfo = context.Message;
            _logger.Information($"OrderBook distributor message received, pair: {pairInfo.UnificatedPairName}");

            if (!_jobManager.IsExist(pairInfo.DistributorId))
            {
                var cts = new CancellationTokenSource();
                await _jobManager.AddAndExecuteInfiniteJob(context.Message.DistributorId, async () =>
                {
                    await _distributorJob.Distribute(_mapper.Map<ExchangePairInfo>(context.Message), (orderBook) =>
                    {
                        _jobManager.HeartBeat(pairInfo.DistributorId, orderBook.Exchange ,orderBook.CatchAt);
                        return Task.CompletedTask;
                    } ,cts.Token);
                }, cts);
            }
            else
            {
                _logger.Warning($"Job with same key: {pairInfo.DistributorId}, already exist.");
            }

            _logger.Information($"Orderbook distributor job: {pairInfo.UnificatedPairName} finished correctly.");
        }
    }
}