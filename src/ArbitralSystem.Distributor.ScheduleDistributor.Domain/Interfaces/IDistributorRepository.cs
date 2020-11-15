using System.Threading;
using System.Threading.Tasks;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Interfaces
{
    public interface IDistributorRepository 
    {
        Task<Models.Distributor> GetAsync(string id, CancellationToken cancellationToken);
        Task<Models.Distributor> CreateAsync(Models.Distributor distributor, CancellationToken cancellationToken);
        Task Delete(string id, CancellationToken cancellationToken);
    }
}