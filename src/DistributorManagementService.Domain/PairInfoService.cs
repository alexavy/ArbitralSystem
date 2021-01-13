using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbitralSystem.Connectors.Core.Arbitral;
using ArbitralSystem.Domain.MarketInfo;

namespace DistributorManagementService.Domain
{
    public class PairInfoService
    {
        private readonly IPublicMarketInfoConnector _marketInfoConnector;

        public PairInfoService(IPublicMarketInfoConnector marketInfoConnector)
        {
            _marketInfoConnector= marketInfoConnector;
        }
        
        public async Task<IEnumerable<(string ExchangePair,Exchange Exchange)>> FindExchangesForPair(string unificatedPair)
        {
            var exchangeAndPairs = new List<(string ExchangePair,Exchange Exchange)>();
            foreach (var exchange in ExchangeHelper.GetAll())
            {
                var pairs =  await _marketInfoConnector.GetPairs(exchange);
                foreach (var pair in pairs.Data)
                {
                    if(pair.ExchangePairName == unificatedPair)
                        exchangeAndPairs.Add( (pair.ExchangePairName, exchange) );
                }
            }
            return exchangeAndPairs;
        }
        
    }
}