using System;
using System.Threading;
using System.Threading.Tasks;
using DistributorManagementService.Domain.Models;

namespace DistributorManagementService.Domain.Interfaces
{
    public interface IOrderBookDistributerRepository
    {
        Task<IOrderBookDistributor> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IOrderBookDistributor> CreateAsync(IOrderBookDistributor orderBookDistributor, CancellationToken cancellationToken);
        Task<IOrderBookDistributor> UpdateAsync(IOrderBookDistributor orderBookDistributor, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}