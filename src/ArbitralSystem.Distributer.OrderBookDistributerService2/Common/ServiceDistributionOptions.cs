using System;
using JetBrains.Annotations;

namespace ArbitralSystem.Distributer.OrderBookDistributerService.Common
{
    [UsedImplicitly]
    public class ServiceDistributionOptions : IServiceDistributionOptions
    {
        public string BotId { get; set; }
        public int? TrimOrderBookDepth { get; set; }
        public ushort PairRetryCount { get; set; }
        public int PairRedeliverSeconds { get;  set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}