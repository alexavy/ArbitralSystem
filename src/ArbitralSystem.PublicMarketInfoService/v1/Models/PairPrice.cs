using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.PublicMarketInfoService.v1.Models
{
    /// <summary>
    /// PairPrice
    /// </summary>
    public class PairPrice
    {
        /// <summary>
        /// Exchange pair name.
        /// </summary>
        public string ExchangePairName { get; set; }

        /// <summary>
        /// Unificated pair name name {BASE/QUOTE}.
        /// </summary>
        public string UnificatedPairName { get; set; }
        
        /// <summary>
        /// Datetime when data saved in system
        /// </summary>
        public DateTimeOffset DateTime { get; set; }
        
        /// <summary>
        /// Price
        /// </summary>
        public decimal? Price { get; set; }
        
        /// <summary>
        /// Exchange name
        /// </summary>
        public Exchange Exchange { get; set; }
    }
}