using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Domain.Interfaces
{
    public interface IDistributerStatesRepository
    {
        Task<DistributerState> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<DistributerState> SaveAsync(DistributerState distributerState, CancellationToken cancellationToken);
    }
}