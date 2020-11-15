using System;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo.Models;

namespace ArbitralSystem.Distributer.OrderBookDistributerService.Common
{
    public static class OrderbookTrimmer
    {
        public static void Trim(IOrderBook orderBook, int count)
        {
            if (count < 0)
                throw new ArgumentException("Cannot trim orderbook, 'count' is not positive value");

            if (orderBook.Asks.Count > count)
                orderBook.Asks.RemoveRange(count, orderBook.Asks.Count - count);

            if (orderBook.Bids.Count > count)
                orderBook.Bids.RemoveRange(count, orderBook.Bids.Count - count);
        }
    }
}