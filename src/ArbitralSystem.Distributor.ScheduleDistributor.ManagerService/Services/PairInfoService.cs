using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Connectors.Core.Arbitral;
using ArbitralSystem.Connectors.Core.Exceptions;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using ArbitralSystem.Domain.MarketInfo;
using AutoMapper;
using Hangfire.Annotations;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Services
{
    [UsedImplicitly]
    public class PairInfoService
    {
        private readonly IPublicMarketInfoConnector _publicMarketInfoConnector;
        private readonly IMapper _mapper;
        
        public PairInfoService(IPublicMarketInfoConnector publicMarketInfoConnector, IMapper mapper)
        {
            Preconditions.CheckNotNull(publicMarketInfoConnector, mapper);
            _publicMarketInfoConnector = publicMarketInfoConnector;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PairInfo>> GetPairInfo(string unificatedPair, Exchange[] exchanges = null)
        {
            var pairs = new List<PairInfo>();
            var currentExchanges = exchanges is null || !exchanges.Any() ? ExchangeHelper.GetAll() : exchanges;
            foreach (var exchange in currentExchanges)
            {
                var exchangePairsResult = await _publicMarketInfoConnector.GetPairs(exchange);
                if (!exchangePairsResult.IsSuccess)
                    throw new RestClientException("Error while getting data from public market data service", exchangePairsResult.Exception);
                pairs.AddRange(_mapper.Map<IEnumerable<PairInfo>>(exchangePairsResult.Data.Where(o => o.UnificatedPairName.Equals(unificatedPair))));
            }
            return pairs;
        }
    }
}