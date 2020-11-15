using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.PublicMarketInfoService.v1.Models
{
    /// <summary>
    /// PairPrice
    /// </summary>
    public class PairPriceSummaryInfo
    {
        /// <summary>
        /// Maximum price.
        /// </summary>
        public decimal? Max { get; set; }

        /// <summary>
        /// Minimum price
        /// </summary>
        public decimal? Min { get; set; }
        
        /// <summary>
        /// Average price
        /// </summary>
        public decimal? Average { get; set; }
        
        /// <summary>
        /// Exchange pair name.
        /// </summary>
        public string ExchangePairName { get; set; }

        /// <summary>
        /// Unificated pair name name {BASE/QUOTE}.
        /// </summary>
        public string UnificatedPairName { get; set; }
        
        /// <summary>
        /// Exchange name
        /// </summary>
        public Exchange Exchange { get; set; }
    }
}