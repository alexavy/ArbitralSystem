using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Connectors.Core.Arbitral;
using ArbitralSystem.Distributer.OrderBookDistributerDomain.Jobs;
using ArbitralSystem.Distributer.OrderBookDistributerDomain.Models;
using Hangfire;

namespace ArbitralSystem.Distributer.OrderBookMultiDistributorDomain.Services
{
    public class DistributorManagerDomainService
    {
        private readonly IPublicMarketInfoConnector _marketInfoConnector;
        private readonly DistributorJob _distributorJob;
        
        public DistributorManagerDomainService(IPublicMarketInfoConnector marketInfoConnector,
        DistributorJob distributorJob)
        {
            Preconditions.CheckNotNull(marketInfoConnector, distributorJob);
            _marketInfoConnector = marketInfoConnector;
            _distributorJob = distributorJob;
        }

        public async Task CreateNewDistributor(string baseCurrency, string quoteCurrency)
        {
            //TODO possible not all pairs in response, default count is 20
            var availablePairs = await _marketInfoConnector.GetPairs(new ArbitralPairInfoFilterFilter()
                {
                    BaseCurrency = baseCurrency,
                    QuoteCurrency = quoteCurrency,
                }
            );

            if(!availablePairs.IsSuccess)
                throw new Exception("Unexpected error in pair info response",availablePairs.Exception);
            var pairs = availablePairs.Data.Items.Select(o => new PairInfo(o.Exchange, o.UnificatedPairName, o.ExchangePairName));
            

            BackgroundJob.Enqueue(() => _distributorJob.Distribute(new ExchangePairInfo(pairs.ToArray()), CancellationToken.None));
        }
    }
}