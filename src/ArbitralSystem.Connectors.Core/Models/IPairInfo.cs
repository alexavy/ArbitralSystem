using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Connectors.Core.Models
{
    public interface IPairInfo : IExchange
    {
        public string UnificatedPairName { get; }

        public string ExchangePairName { get; }

        public string BaseCurrency { get; }

        public string QuoteCurrency { get; }
    }
}