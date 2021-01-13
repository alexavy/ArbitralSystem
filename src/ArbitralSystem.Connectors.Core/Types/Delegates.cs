using ArbitralSystem.Connectors.Core.Models;

namespace ArbitralSystem.Connectors.Core.Types
{
    public delegate void OrderBookDelegate(IOrderBook orderBook);
    public delegate void DistributerStateDelegate(IDistributerState state);
}