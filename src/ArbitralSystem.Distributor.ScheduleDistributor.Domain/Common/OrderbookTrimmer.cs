using System;
using System.Collections.Generic;
using System.Linq;
using ArbitralSystem.Connectors.Core.Models;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Common
{
    internal static class OrderbookTrimmer
    {
        public static IOrderBook Trim(IOrderBook orderBook, int count)
        {
            if (count < 1)
                throw new ArgumentException("Cannot trim orderbook, 'count' is not positive value, and greater than '1'.");

            if (orderBook.Asks.Count() < count && orderBook.Bids.Count() < count)
                return orderBook;

            var asks = new List<IOrderbookEntry>(orderBook.Asks);
            asks.RemoveRange(count, asks.Count - count);
            
            var bids = new List<IOrderbookEntry>(orderBook.Bids);
            bids.RemoveRange(count, bids.Count - count);

            return new OrderBook(orderBook.Exchange, orderBook.Symbol, orderBook.CatchAt, bids, asks, orderBook.BestBid, orderBook.BestAsk);
        }
    }
}