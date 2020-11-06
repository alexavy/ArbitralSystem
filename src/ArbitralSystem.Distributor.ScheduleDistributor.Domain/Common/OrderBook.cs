using System;
using System.Collections.Generic;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Common
{
    internal class OrderBook : IOrderBook
    {
        public Exchange Exchange { get; }
        public string Symbol { get; }
        public DateTimeOffset CatchAt { get; }
        public IEnumerable<IOrderbookEntry> Bids { get; }
        public IEnumerable<IOrderbookEntry> Asks { get; }
        public IOrderbookEntry BestBid { get; }
        public IOrderbookEntry BestAsk { get; }

        public OrderBook(Exchange exchange,
            string symbol,
            DateTimeOffset catchAt,
            IEnumerable<IOrderbookEntry> bids,
            IEnumerable<IOrderbookEntry> asks,
            IOrderbookEntry bestBid,
            IOrderbookEntry bestAsk)
        {
            Exchange = exchange;
            Symbol = symbol;
            CatchAt = catchAt;
            Bids = bids;
            Asks = asks;
            BestBid = bestBid;
            BestAsk = bestAsk;
        }
    }
}