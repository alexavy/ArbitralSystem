using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Distributer.PairInfoDistributerService.Options;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Domain.MarketInfo.Models;
using ArbitralSystem.Messaging.Common;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Service.Core.Messaging;
using ArbitralSystem.Storage.RemoteCacheStorage;
using Microsoft.Extensions.Hosting;

[assembly: InternalsVisibleTo("ArbitralSystem.Distributer.PairInfoDistributerService.Test")]

namespace ArbitralSystem.Distributer.PairInfoDistributerService
{
    internal class ControlService : IHostedService
    {
        private readonly IPublicConnectorFactory _connectorFactory;
        private readonly IPairInfoDistributerOptions _distributerOptions;
        private readonly ILogger _logger;
        private readonly IPairCacheStorage _pairCacheStorage;
        private readonly IDomainBusProducer _runCommand;

        public ControlService(IPublicConnectorFactory connectorFactory,
            IPairInfoDistributerOptions distributerOptions,
            IPairCacheStorage pairCacheStorage,
            IDomainBusProducer runCommand,
            ILogger logger)
        {
            _distributerOptions = distributerOptions;
            _connectorFactory = connectorFactory;
            _pairCacheStorage = pairCacheStorage;
            _runCommand = runCommand;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Control service started");
            var pairs = await GetTotalPairs();
            var siftedPairs = SiftPairInfos(pairs);

            var counter = 0;
            foreach (var pair in siftedPairs)
            {
                counter++;
                _logger.Information("Run command for pair: {@pair}", pair);
                var runBotOnPair = new PairInfoMessage(pair);
                await _runCommand.PublishAsync(runBotOnPair);
            }

            _logger.Information($"Total pairs sent: {counter}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Control service stopped");
            return Task.CompletedTask;
        }

        private async Task<IEnumerable<IPairInfo>> GetTotalPairs()
        {
            var totalPairs = new List<IPairInfo>();
            foreach (Exchange exchange in ExchangeHelper.GetAll())
            {
                try
                {
                    var exchangePairs = await GetTotalPairsForExchange(exchange);
                    totalPairs.AddRange(exchangePairs);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Error while getting pairs for {exchange}");
                }
            }

            return totalPairs;
        }

        private IEnumerable<IPairInfo> SiftPairInfos(IEnumerable<IPairInfo> totalPairs)
        {
            switch (_distributerOptions.SiftType)
            {
                case SiftType.ListedMoreThenTwoExchanges:
                {
                    return totalPairs.GroupBy(o => o.UnificatedPairName)
                        .Where(o => o.Count() > 1)
                        .Select(o => o.First());
                }
                case SiftType.UniquePair:
                {
                    return totalPairs.GroupBy(o => o.UnificatedPairName)
                        .Select(o => o.First());
                }
                default:
                    throw new Exception("Not supported sift type");
            }
        }

        private async Task<IList<IPairInfo>> GetTotalPairsForExchange(Exchange exchange)
        {
            var cachePairs = await TryGetPairsFromCache(exchange);
            if (cachePairs != null && cachePairs.Any())
            {
                _logger.Information($"Restored pairs for {exchange} from cache, total : {cachePairs.Count}");
                return cachePairs;
            }

            var connectorInstance = _connectorFactory.GetInstance(exchange);
            var exchangePairs = await connectorInstance.GetPairsInfo();
            var pairs = exchangePairs.ToList();
            _logger.Information($"Pairs successfully received from {exchange}, total : {pairs.Count}");
            pairs.ForEach(o => _pairCacheStorage.SetPairAsync(o));
            return pairs;
        }

        private async Task<IList<IPairInfo>> TryGetPairsFromCache(Exchange exchange)
        {
            try
            {
                return await _pairCacheStorage.GetAllPairsAsync(exchange);
            }
            catch (Exception ex)
            {
                _logger.Warning(ex, $"Cache is not available for {exchange}");
                return null;
            }
        }
    }
}