using System;

namespace ArbitralSystem.Distributer.OrderBookDistributerService.Common
{
    public interface IServiceDistributionOptions : ICloneable
    {
        int? TrimOrderBookDepth { get; }

        ushort PairRetryCount { get; }

        int PairRedeliverSeconds { get; }
    }
}