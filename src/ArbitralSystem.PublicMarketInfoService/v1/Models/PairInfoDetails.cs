using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.PublicMarketInfoService.v1.Models
{
    /// <summary>
    /// Details
    /// </summary>
    public class PairInfoDetails
    {
        /// <summary>
        /// Exchange pair name.
        /// </summary>
        public string ExchangePairName { get; set; }
        
        /// <summary>
        /// In what time pair was received in service
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// In what time pair was delisted in service
        /// </summary>
        public DateTimeOffset? DelistedAt { get; set; }
        
        /// <summary>
        /// Exchange name
        /// </summary>
        public Exchange Exchange { get; set; }
    }
}