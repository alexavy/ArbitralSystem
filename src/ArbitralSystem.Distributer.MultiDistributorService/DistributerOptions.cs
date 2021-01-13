using ArbitralSystem.Connectors.CryptoExchange.Common;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributer.OrderBookMultiDistributorService
{
    public class DistributerOptions : IDistributerOptions
    {
        public object Clone()
        {
            throw new System.NotImplementedException();
        }

        public Exchange Exchange { get; }
        public int Frequency { get; }
        public int? Limit { get; }
    }
}