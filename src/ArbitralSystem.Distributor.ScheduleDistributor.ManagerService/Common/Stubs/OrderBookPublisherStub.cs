
using System.Threading.Tasks;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Interfaces;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Common.Stubs
{
    internal class OrderBookPublisherStub : IOrderBookPublisher
    {
        public Task Publish(IOrderBook orderbook)
        {
            throw new StubNotSupportedException();
        }

        public Task Publish(IDistributerState orderBookState)
        {
            throw new StubNotSupportedException();
        }
    }
}