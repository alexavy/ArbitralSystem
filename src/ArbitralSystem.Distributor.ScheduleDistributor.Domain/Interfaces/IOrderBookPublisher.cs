using System.Threading.Tasks;
using ArbitralSystem.Connectors.Core.Models;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Interfaces
{
    public interface IOrderBookPublisher
    {
        Task Publish(IOrderBook orderbook);
        Task Publish(IDistributerState orderBookState);
    }
}