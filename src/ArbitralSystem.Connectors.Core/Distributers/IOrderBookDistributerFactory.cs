using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Connectors.Core.Distributers
{
    public interface IOrderBookDistributerFactory
    {
        IOrderbookDistributor GetInstance(Exchange exchange);
    }
}
