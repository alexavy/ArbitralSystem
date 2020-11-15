using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Domain.MarketInfo.Models;
using ArbitralSystem.Messaging.Common;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Service.Core;
using ArbitralSystem.Service.Core.Messaging;
using Microsoft.Extensions.Hosting;

namespace ArbitralSystem.Distributer.PairPolygonDistributerService
{
    internal class TriangleService : IHostedService
    {
        private readonly IExtendedExchangeConnector _extendedExchange;
        private readonly ILogger _logger;
        private readonly IDomainBusProducer _producer;

        public TriangleService(IExtendedExchangeConnector extendedExchange,
            IDomainBusProducer producer,
            ILogger logger)
        {
            _extendedExchange = extendedExchange;
            _producer = producer;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            /*var allExchanges = ExchangeHelper.GetAll();
            var totalPairs = await _extendedExchange.GetTotalPairsFromCacheOrExchange(allExchanges.ToArray());

            var groupedTotalPairs = totalPairs.GroupBy(o => o.Exchange);
            foreach (var exchangePairs in groupedTotalPairs)
            {
                var triangles = FindAllTrianglesForExchange(exchangePairs);

                foreach (var triangle in triangles) await _producer.PublishAsync(new PairPolygonMessage(triangle));
            }*/
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
        
       /* private IEnumerable<IPairPolygon> FindAllTrianglesForExchange(IEnumerable<IPairInfo> totalExchangePairs)
        {
            var triangles = new List<IPairPolygon>();
            foreach (var pair in totalExchangePairs)
            {
                var firstPosibleCorners = totalExchangePairs.Where(o => o.QuoteCurrency == pair.QuoteCurrency);

                foreach (var firstPosibleCorner in firstPosibleCorners)
                {
                    var secondCorners = totalExchangePairs.Where(o =>
                        o.QuoteCurrency == firstPosibleCorner.BaseCurrency && o.BaseCurrency == pair.BaseCurrency);

                    if (secondCorners.Any())
                    {
                        if (secondCorners.Count() > 1)
                            _logger.Warning("UnNormal situation, more then one last corners in triangle");

                        var polygon = new PairPolygon();

                        polygon.Exchange = pair.Exchange;
                        polygon.PolygonPairs.Add(pair);
                        polygon.PolygonPairs.Add(firstPosibleCorner);
                        polygon.PolygonPairs.Add(secondCorners.First());

                        triangles.Add(polygon);
                    }
                }
            }

            return triangles.GroupBy(plg => plg.PairsPolygonIdentity).Select(o => o.First());
        }*/
    }
}