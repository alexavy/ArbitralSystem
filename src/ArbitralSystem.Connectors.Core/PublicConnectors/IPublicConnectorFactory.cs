using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Connectors.Core.PublicConnectors
{
    public interface IPublicConnectorFactory
    {
        IPublicConnector GetInstance(Exchange exchange);
    }
}
