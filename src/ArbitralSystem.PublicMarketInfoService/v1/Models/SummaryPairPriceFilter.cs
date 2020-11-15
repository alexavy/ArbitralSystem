using System;

namespace ArbitralSystem.PublicMarketInfoService.v1.Models
{
    public class SummaryPairPriceFilter 
    {
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }
    }
}