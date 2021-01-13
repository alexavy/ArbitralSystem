using System;

namespace ArbitralSystem.Distributor.MarketInfoAggregatorService.Common
{
    public class MarketInfoAggregatorOptions : ICloneable
    {
        public ushort PrefetchCount { get; set; }
        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}