using System;
using ArbitralSystem.Domain.Distributers;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models
{
    public class DistributerState 
    {
        public string Symbol { get; }
        public Exchange Exchange { get; }
        public DateTimeOffset ChangedAt { get; }
        public DistributerSyncStatus PreviousStatus { get; }
        public DistributerSyncStatus CurrentStatus { get; }

        public DistributerState(string symbol, Exchange exchange, DateTimeOffset changedAt, DistributerSyncStatus previousStatus,
            DistributerSyncStatus currentStatus)
        {
            if (!symbol.Contains('/'))
                throw new ArgumentException("Symbol must be in unificated format");

            Symbol = symbol;

            if (exchange == Exchange.Undefined)
                throw new ArgumentException("Exchange can not be undefined.");

            Exchange = exchange;
            ChangedAt = changedAt;
            PreviousStatus = previousStatus;
            CurrentStatus = currentStatus;
        }
    }
}