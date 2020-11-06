using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Interfaces
{
    public interface IDistributorRepository
    {
        Task<Models.Distributor> GetAsync(Guid id, CancellationToken token);
        Task<Models.Distributor> CreateAsync(Models.Distributor distributor, CancellationToken token);
        Task UpdateAsync(Models.Distributor distributor, CancellationToken token);
        Task DeleteAsync(Guid id, CancellationToken token);
    }
}