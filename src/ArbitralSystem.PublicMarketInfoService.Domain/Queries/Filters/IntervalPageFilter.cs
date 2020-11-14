using System;
using ArbitralSystem.Common.Pagination;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters
{
    public class IntervalPageFilter : PageFilter
    {
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }
    }
}