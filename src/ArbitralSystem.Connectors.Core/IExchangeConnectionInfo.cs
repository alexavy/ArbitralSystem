using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Connectors.Core
{

    public interface IConnectionInfo
    {
        string BaseRestUrl { get; }
        string ApiKey { get; }
        string ApiSecret { get; }
        int DefaultTimeOutInMs { get; }
    }
    
    public interface IExchangeConnectionInfo : IExchange , IConnectionInfo
    {
    }
}