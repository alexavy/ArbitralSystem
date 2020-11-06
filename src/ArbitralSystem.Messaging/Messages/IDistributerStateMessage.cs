using System;
using ArbitralSystem.Domain.Distributers;

using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Messaging.Messages
{
    public interface IDistributerStateMessage :  ICorrelation
    {
        string Symbol { get; }
        Exchange Exchange { get; }
        DateTimeOffset ChangedAt { get;  }
        DistributerSyncStatus PreviousStatus { get;  }
        DistributerSyncStatus CurrentStatus { get; }
    }
}
