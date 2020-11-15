using System.Threading.Tasks;
using ArbitralSystem.Connectors.Core.Models;

namespace ArbitralSystem.Distributor.Core.Interfaces
{
    public interface IOrderBookPublisher
    {
        Task Publish(IOrderBook orderbook);
        Task Publish(IDistributerState orderBookState);
    }
}