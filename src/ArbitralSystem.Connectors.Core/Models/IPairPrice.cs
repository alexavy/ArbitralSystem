using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Connectors.Core.Models
{
    public interface IPairPrice : IExchange
    {
        public string ExchangePairName { get; }
        public decimal? Price { get; }
    }
}