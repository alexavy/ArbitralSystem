using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Common;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Interfaces;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using AutoMapper;
using Hangfire;
using Hangfire.Server;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Jobs
{
    public class OrderBookDistributorJob 
    {
        private readonly IOrderBookDistributerFactory _orderBookDistributer;
        private readonly DistributionOptions _distributionOptions;
        private readonly IOrderBookPublisher _publisher;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        
        
        public OrderBookDistributorJob(IOrderBookDistributerFactory orderBookDistributer,
            DistributionOptions distributionOptions,
            IOrderBookPublisher publisher,
            ILogger logger,
            IMapper mapper)
        {
            Preconditions.CheckNotNull(orderBookDistributer, distributionOptions, publisher, logger, mapper);
            _orderBookDistributer = orderBookDistributer;
            _distributionOptions = distributionOptions;
            _publisher = publisher;
            _logger = logger;
            _mapper = mapper;
        }
        
        //[DisableConcurrentExecution(100)]
        [Queue("orderbook-queue")]
        public async Task Distribute(DistributorExchangePairs distributorExchangePairs, CancellationToken token)
        {
            var exchangePairsInfo = distributorExchangePairs.ExchangePairInfo;
            //var cancellationDistributionTokenSource =  new CancellationTokenSource();//CancellationTokenSource.CreateLinkedTokenSource(token);
            var cancellationDistributionTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
            var availableDistributers = new List<IOrderbookDistributor>();
            try
            {
                var availablePairs = exchangePairsInfo.PairInfos.ToArray();
                _logger.Debug("Total available pairs: {count}, Extended: {exchanges}"
                    , availablePairs.Count(), String.Join(" , ", availablePairs.Select(o=>o.Exchange).ToArray()) );
                
                availableDistributers = GetAvailableDistributers(availablePairs);
                
                _logger.Debug("Start distribution");
                await StartDistribution(availableDistributers, availablePairs, cancellationDistributionTokenSource.Token);

            }
            catch (Exception ex)
            {
                cancellationDistributionTokenSource.Cancel();
                _logger.Fatal(ex, "Fatal error while distribution");
                throw;
            }
            finally
            {
                _logger.Information("Unsubscription started.");
                if (availableDistributers.Any())
                    UnSubscribeFromDistributers(availableDistributers);
                _logger.Information("Stopped distribution");
            }
        }
        
        private async void Instance_OrderBookChanged(IOrderBook orderBook)
        {
            var currentOrderBook = _distributionOptions.TrimOrderBookDepth.HasValue
                ? OrderbookTrimmer.Trim(orderBook, _distributionOptions.TrimOrderBookDepth.Value) : orderBook;

            await _publisher.Publish(currentOrderBook);
            
            //_logger.Information(
              //  $"Orderbook updated for pair {currentOrderBook.Symbol}, asks:{currentOrderBook.Asks.Count()}, bids:{currentOrderBook.Bids.Count()}, from {currentOrderBook.Exchange}");
        }
        
        private async void Instance_DistributerStateChanged(IDistributerState state)
        {
            await _publisher.Publish(state);
            _logger.Information("distributer state updated: {@state}", state);
        }
        
        private List<IOrderbookDistributor> GetAvailableDistributers(IEnumerable<PairInfo> availablePairs)
        {
            var availableDistributers = new List<IOrderbookDistributor>();
            foreach (var pair in availablePairs)
            {
                var instance = _orderBookDistributer.GetInstance(pair.Exchange);
                instance.OrderBookChanged += Instance_OrderBookChanged;
                instance.DistributerStateChanged += Instance_DistributerStateChanged;

                _logger.Debug("Preparing distribution for Exchange: {@exchanges}", pair.Exchange);

                availableDistributers.Add(instance);
            }
            return availableDistributers;
        }
        
        private async Task StartDistribution(IEnumerable<IOrderbookDistributor> distributers,PairInfo[] pairs ,CancellationToken token)
        {
            var activeDistributers = new List<Task>();
            foreach (var distributer in distributers)
            {
                var pair = pairs.SingleOrDefault(o => o.Exchange == distributer.Exchange);
                var distributerPair = _mapper.Map<OrderBookPairInfo>(pair);
                activeDistributers.Add(await distributer.StartDistributionAsync(distributerPair, token));
            }
            await Task.WhenAll(activeDistributers);
        }

        private void UnSubscribeFromDistributers(List<IOrderbookDistributor> distributers)
        {
            foreach (var distributer in distributers)
            {
                distributer.OrderBookChanged -= Instance_OrderBookChanged;
                distributer.DistributerStateChanged -= Instance_DistributerStateChanged;
            }
        }

    }
}