using System.Collections.Generic;
using System.Threading.Tasks;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Service.Core
{
    public interface IExtendedExchangeConnector
    {
        Task<IEnumerable<IPairInfo>> GetTotalPairsFromCacheOrExchange(Exchange[] exchanges);

        Task<IList<IPairInfo>> GetTotalPairsFromCacheOrExchange(Exchange exchange);
    }
}
