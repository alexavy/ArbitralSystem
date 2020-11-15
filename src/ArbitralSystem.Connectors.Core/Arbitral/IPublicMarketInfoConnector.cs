using System.Collections.Generic;
using System.Threading.Tasks;
using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Connectors.Core.Common;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Connectors.Core.Arbitral
{
    public interface IPublicMarketInfoConnector
    {
        Task<IResponse<IEnumerable<IArbitralPairInfo>>> GetPairs(Exchange exchange);
        
        Task<IResponse<IPage<IArbitralPairInfo>>> GetPairs(ArbitralPairInfoFilterFilter filter);
    }
}