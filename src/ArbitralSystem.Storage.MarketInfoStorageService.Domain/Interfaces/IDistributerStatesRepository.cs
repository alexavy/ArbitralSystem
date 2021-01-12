using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Domain.Interfaces
{
    public interface IDistributerStatesRepository
    {
        Task SaveWithNoCheckAsync(DistributerState distributerState, CancellationToken cancellationToken);
    }
}