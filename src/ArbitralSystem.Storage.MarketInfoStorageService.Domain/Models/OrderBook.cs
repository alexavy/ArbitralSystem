using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models
{
    public class OrderBook : IExchange
    {
        public Guid Id { get; }
        public string Symbol { get;  }
        public Exchange Exchange { get; }
        public DateTimeOffset CatchAt { get; }
        public IEnumerable<OrderbookEntry> Bids { get; }
        public IEnumerable<OrderbookEntry> Asks { get; }

        public OrderBook(string symbol, Exchange exchange, DateTimeOffset catchAt, IEnumerable<OrderbookEntry> asks, IEnumerable<OrderbookEntry> bids)
        {
            Id = Guid.NewGuid();
            
            if(!symbol.Contains('/'))
                throw new ArgumentException("Symbol must be in unificated format");

            Symbol = symbol;
                
            if(exchange == Exchange.Undefined)
                throw new ArgumentException("Exchange can not be undefined.");

            Exchange = exchange;
            CatchAt = catchAt;

            ValidateOrderBookEntries(bids);
            ValidateOrderBookEntries(asks);
            
            Asks = asks;
            Bids = bids;
        }

        private void ValidateOrderBookEntries(IEnumerable<OrderbookEntry> entries)
        {
            if(entries.GroupBy(o=>o.Price).Any(x=>x.Count() > 1))
                throw new ArgumentException("Price in orderbook entries must be unique!");
        }
    }
}