using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Storage.RemoteCacheStorage
{
    public interface IPairCacheStorage: IDisposable
    {
        Task<bool> SetPairAsync(IPairInfo pair);

        Task<IList<IPairInfo>> GetAllPairsAsync();

        Task<IList<IPairInfo>> GetAllPairsAsync(Exchange exchange);
    }
}
