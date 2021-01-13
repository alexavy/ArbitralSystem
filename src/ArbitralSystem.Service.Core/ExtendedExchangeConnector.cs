using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Service.Core.Exceptions;
using ArbitralSystem.Storage.RemoteCacheStorage;
using JetBrains.Annotations;

namespace ArbitralSystem.Service.Core
{
    public class ExtendedExchangeConnector : IExtendedExchangeConnector
    {
        private readonly IPublicConnectorFactory _connectorFactory;
        private readonly ILogger _logger;
        private readonly IPairCacheStorage _pairCacheStorage;

        public ExtendedExchangeConnector([NotNull] IPublicConnectorFactory connectorFactory,
            [NotNull] IPairCacheStorage pairCacheStorage,
            [NotNull] ILogger logger)
        {
            _connectorFactory = connectorFactory;
            _pairCacheStorage = pairCacheStorage;
            _logger = logger;
        }

        public async Task<IEnumerable<IPairInfo>> GetTotalPairsFromCacheOrExchange(Exchange[] exchanges)
        {
            var totalPairs = new List<IPairInfo>();
            foreach (var exchange in exchanges)
            {
                if (exchange == Exchange.Undefined)
                    continue;

                try
                {
                    var exchangePairs = await GetTotalPairsFromCacheOrExchange(exchange);
                    totalPairs.AddRange(exchangePairs);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Error while getting pairs for {exchange}");
                    throw new ServiceException($"Error while getting pairs for {exchange}", ex);
                }
            }

            return totalPairs;
        }

        public async Task<IList<IPairInfo>> GetTotalPairsFromCacheOrExchange(Exchange exchange)
        {
            var cachePairs = await TryGetPairsFromCache(exchange);
            if (cachePairs != null && cachePairs.Any())
            {
                _logger.Debug($"Restored pairs for {exchange} from cache, total : {cachePairs.Count}");
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