using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Distributer.OrderBookDistributerService.Common;
using ArbitralSystem.Distributer.OrderBookDistributerService.Models;
using ArbitralSystem.Distributer.OrderBookDistributerService.Workflow;
using ArbitralSystem.Messaging.Common;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Service.Core.Messaging;
using AutoMapper;
using JetBrains.Annotations;
using MassTransit;

namespace ArbitralSystem.Distributer.OrderBookDistributerService.Consumers
{
    [UsedImplicitly]
    internal class ExchangePairInfoConsumer : IConsumer<IExchangePairInfoMessage>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrderBookDistributerWorkflow _orderBookDistributerWorkflow;
        private readonly IServiceDistributionOptions _serviceDistributionOptions;

        public ExchangePairInfoConsumer(IOrderBookDistributerWorkflow orderBookDistributerWorkflow,
            IServiceDistributionOptions serviceDistributionOptions,
            IBusControl busControl,
            IDomainBusProducer busProducer,
            ILogger logger ,IMapper mapper)
        {
            _orderBookDistributerWorkflow = orderBookDistributerWorkflow;
            _serviceDistributionOptions = serviceDistributionOptions;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IExchangePairInfoMessage> context)
        {
            _logger.Information(
                "Pair accepted, retry attempt: {retryAttempt}, redelivery count: {redeliveryCount}, Pair: {@pair}",
                context.GetRetryAttempt(),
                context.GetRedeliveryCount(),
                context.Message);
                
            if (_orderBookDistributerWorkflow.GetState() == DistributerWorkflowState.Listening)
            {
                await HandlePairAndRedeliverAfterAllReties(context);
            }
            else
            {
                var warningMessage = "Bot in wrong state for receiving pairs";
                _logger.Warning(warningMessage);
                throw new Exception(warningMessage);
            }
        }

        private async Task HandlePairAndRedeliverAfterAllReties(ConsumeContext<IExchangePairInfoMessage> context)
        {
            try
            {
                _logger.Information("Start distribution.");
                var exchangePairInfo = _mapper.Map<ExchangePairInfo>(context.Message);
                await _orderBookDistributerWorkflow.StartDistribution(exchangePairInfo);
            }
            catch (Exception e)
            {
                _logger.Error(e,"UnExpected error in distribution process");
                await _orderBookDistributerWorkflow.ContinueListening();

                if (context.GetRetryAttempt() >= _serviceDistributionOptions.PairRetryCount)
                {
                    _logger.Warning($"All retry attempts are used, pair will be redelivered through {_serviceDistributionOptions.PairRedeliverSeconds} seconds ");
                    await context.Redeliver(TimeSpan.FromSeconds(_serviceDistributionOptions.PairRedeliverSeconds));
                    return;
                }
                throw;
            }
        }
    }
}