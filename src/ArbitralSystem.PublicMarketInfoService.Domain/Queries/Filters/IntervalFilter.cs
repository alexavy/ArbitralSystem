using System;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters
{
    public class IntervalFilter
    {
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }
    }
}