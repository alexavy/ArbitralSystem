using System.Threading;

namespace ArbitralSystem.Connectors.CryptoExchange.Common
{
    public class OrderBookDistributerInstance<T>
    {
        public string InstanceSymbol { get; set; }

        public T OrderBook { get; set; }

        public CancellationTokenSource TokenSource { get; set; }
    }
}