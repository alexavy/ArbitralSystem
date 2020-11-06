using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Interfaces
{
    public interface IServerRepository
    {
        Task<Models.Server> GetAsync(Guid id, CancellationToken token);
        Task<Models.Server> CreateAsync(Models.Server server, CancellationToken token);
        Task UpdateAsync(Models.Server server, CancellationToken token);
    }
}