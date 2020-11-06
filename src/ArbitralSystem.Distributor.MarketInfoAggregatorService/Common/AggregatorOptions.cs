using System;

namespace ArbitralSystem.Distributor.MarketInfoAggregatorService.Common
{
    public class AggregatorOptions : ICloneable
    {
        public int Limit { get; set; }
        public TimeSpan TimeBaseCleansing { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}