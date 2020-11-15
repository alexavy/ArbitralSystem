using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Interfaces;
using ArbitralSystem.PublicMarketInfoService.Domain.Models;
using AutoMapper;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Services
{
    public class PairPricesDomainService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPairPricesRepository _pairPricesRepository;
        private readonly IPublicConnectorFactory _publicConnectorFactory;

        public PairPricesDomainService(ILogger logger, IMapper mapper, IPairPricesRepository pairPricesRepository,
            IPublicConnectorFactory publicConnectorFactory)
        {
            Preconditions.CheckNotNull(logger, mapper, pairPricesRepository, publicConnectorFactory);

            _logger = logger;
            _mapper = mapper;
            _pairPricesRepository = pairPricesRepository;
            _publicConnectorFactory = publicConnectorFactory;
        }

        public async Task SaveLastPrices(IEnumerable<Exchange> exchanges, CancellationToken token)
        {
            _logger.Information("Saving pair prices for {@exchanges} started.", exchanges);
            var pricesTasks = new  List<Task<IEnumerable<IPairPrice>>>();
            foreach (var exchange in exchanges)
            {
                var connector = _publicConnectorFactory.GetInstance(exchange);
                pricesTasks.Add(connector.GetPairPrices(token));
            }

            var prices = (await Task.WhenAll(pricesTasks))
                .SelectMany(o=>o)
                .Select(o => new PairPrice(o.ExchangePairName, o.Price, o.Exchange))
                .ToArray();
            _logger.Information($"Pair prices pulled, total: {prices.Count()}.");
            await _pairPricesRepository.BulkSave(prices, token);
            _logger.Information($"Pair prices successfully saved.");
        }
    }
}