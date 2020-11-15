using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Common.Stubs
{
    internal class OrderBookDistributerFactoryStub : IOrderBookDistributerFactory
    {
        public IOrderbookDistributor GetInstance(Exchange exchange)
        {
            throw new StubNotSupportedException();
        }
    }
}