using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Interfaces
{
    public interface IHeartBeatRepository
    {
        Task Update(DistributorHeartBeat[] heartBeats, CancellationToken token);
    }
}