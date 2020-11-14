using System;
using ArbitralSystem.PublicMarketInfoService.v1.Models.Paging;


namespace ArbitralSystem.PublicMarketInfoService.v1.Models
{

    /// <summary>
    /// Pair price filter
    /// </summary>
    public class PairPriceFilter : PageFilter
    {
        /// <summary>
        /// Select from date
        /// </summary>
        public DateTimeOffset? From { get; set; }
        
        /// <summary>
        /// Select to date
        /// </summary>
        public DateTimeOffset? To { get; set; }
    }
}