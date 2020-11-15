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
using ArbitralSystem.Messaging.Common;
using ArbitralSystem.Messaging.Messages;
using AutoMapper;
using PairInfo = ArbitralSystem.Distributer.OrderBookDistributerService.Models.PairInfo;

namespace ArbitralSystem.Distributer.OrderBookDistributerService.Services
{

    internal interface IDistributorService
    {
        Task Distribute(ExchangePairInfo exchangePairInfo, CancellationToken token);
    }
    
    internal class DistributorService
    {
        private readonly IServiceDistributionOptions _serviceDistributionOptions;
        private readonly IOrderBookDistributerFactory _orderBookDistributer;
        private readonly IDomainBusProducer _busProducer;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        
        
        public DistributorService()
        {
            
        }

        public async Task Distribute(ExchangePairInfo exchangePairInfo, CancellationToken token)
        {
            var cancellationDistributionTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
            
            var availableDistributers = new List<IOrderbookDistributor>();
            try
            {
                var availablePairs = exchangePairInfo.PairInfos.ToArray();
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
                if (availableDistributers.Any())
                    UnSubscribeFromDistributers(availableDistributers);
            }
        }
        
                private async void Instance_OrderBookChanged(IOrderBook orderBook)
        {
            if (_serviceDistributionOptions.TrimOrderBookDepth.HasValue)
                if (orderBook.Asks.Count() > _serviceDistributionOptions.TrimOrderBookDepth.Value ||
                    orderBook.Bids.Count() > _serviceDistributionOptions.TrimOrderBookDepth.Value)
                    OrderbookTrimmer.Trim(orderBook, _serviceDistributionOptions.TrimOrderBookDepth.Value);
            await _busProducer.PublishAsync( _mapper.Map<OrderBookMessage>(orderBook));

            _logger.Information(
                $"Orderbook updated for pair {orderBook.Symbol}, asks:{orderBook.Asks.Count}, bids:{orderBook.Bids.Count}, from {orderBook.Exchange}");
        }


        private async void Instance_DistributerStateChanged(IDistributerState state)
        {
            await _busProducer.PublishAsync(_mapper.Map<DistributerStateMessage>(state));
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
                var distributerPair = _mapper.Map<IPairInfo>(pair);
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