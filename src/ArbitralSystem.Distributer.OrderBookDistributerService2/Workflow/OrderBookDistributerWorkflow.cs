using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Distributer.OrderBookDistributerService.Common;
using ArbitralSystem.Distributer.OrderBookDistributerService.Models;
using ArbitralSystem.Distributer.OrderBookDistributerService.Producers;
using ArbitralSystem.Distributer.OrderBookDistributerService.Services;
using ArbitralSystem.Domain.Distributers;
using ArbitralSystem.Domain.MarketInfo.Models;
using ArbitralSystem.Messaging.Common;
using ArbitralSystem.Messaging.Messages;
using AutoMapper;
using MassTransit;
using Stateless;
using PairInfo = ArbitralSystem.Distributer.OrderBookDistributerService.Models.PairInfo;

namespace ArbitralSystem.Distributer.OrderBookDistributerService.Workflow
{
    internal class OrderBookDistributerWorkflow 
    {
        private readonly IDistributerManagerService _managerService;
        private readonly IDistributorService _distributorService;
        private readonly IDomainBusProducer _busProducer;
        private readonly IBusControl _busControl;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private readonly BotMetaData _botMetaData;
        
        private readonly StateMachine<DistributorState, DistributerTrigger> _distributerMachine;
        
        private readonly StateMachine<DistributorState, DistributerTrigger>.TriggerWithParameters<ExchangePairInfo>
            _startDistributionTrigger;

        private Task CurrentTak;

        private CancellationTokenSource _cancellationTokenSource;


        public OrderBookDistributerWorkflow(IOrderBookDistributerFactory orderBookDistributer,
            IServiceDistributionOptions serviceDistributionOptions,
            IDomainBusProducer busProducer,
            IBusControl busControl,
            IMapper mapper,
            ILogger logger)
        {
            _orderBookDistributer = orderBookDistributer;
            _serviceDistributionOptions = serviceDistributionOptions;
            _busProducer = busProducer;
            _busControl = busControl;
            _mapper = mapper;
            _logger = logger;

            _distributerMachine = new StateMachine<DistributorState, DistributerTrigger>(DistributorState.Initialization);
            _startDistributionTrigger = _distributerMachine.SetTriggerParameters<ExchangePairInfo>(DistributerTrigger.StartDistribution);

            _distributerMachine.Configure(DistributorState.Initialization)
                .OnEntryAsync(async () => await StateNotify())
                .Permit(DistributerTrigger.StartListenForPair, DistributorState.Listening);

            _distributerMachine.Configure(DistributorState.Listening)
                .OnEntryAsync(async () => await StateNotify())
                .Permit(DistributerTrigger.StartDistribution, DistributorState.Distribution);

            _distributerMachine.Configure(DistributorState.Distribution)
                .OnEntryAsync(async () => await StateNotify())
                .OnEntryFromAsync(_startDistributionTrigger, async pair => await OnDistribution(pair))
                .Permit(DistributerTrigger.StopDistribution, DistributorState.Stopped);
            
            _distributerMachine.Configure(DistributorState.Stopped)
                .OnEntryAsync(async () => await StateNotify())
                .OnEntryFromAsync(_startDistributionTrigger, async pair => await OnDistribution(pair))
                .Permit(DistributerTrigger.StartListenForPair, DistributorState.Listening);

        }

        private Task _distributorTask;
        private CancellationTokenSource _cancellationDistributionTokenSource;
        
        public DistributorState GetState()
        {
            return _distributerMachine.State;
        }

        public async Task StartListeningForPairs()
        {
            await _distributerMachine.FireAsync(DistributerTrigger.StartListenForPair);
        }
        
        public async Task StartDistribution(ExchangePairInfo pairInfo)
        {
            _cancellationDistributionTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token);
            await _distributerMachine.FireAsync(_startDistributionTrigger, pairInfo);
        }
        
        public async Task StopDistribution()
        {
            _cancellationDistributionTokenSource.Cancel();
            await _distributorTask;
        }
        
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _busControl.Stop();
            _logger.Debug("Listening Stopped");
        }

        private async Task OnDistribution(ExchangePairInfo exchangePairInfo)
        {
            _distributorTask = _distributorService.Distribute(exchangePairInfo, _cancellationDistributionTokenSource.Token);
        }
        
        private async Task StateNotify()
        {
            await _managerService.Notify(_botMetaData.ChangeState(GetState()));
        }
    }
}