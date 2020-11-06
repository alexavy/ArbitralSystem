using System.Collections.Generic;

namespace ArbitralSystem.PublicMarketInfoService.v1.Models
{
    /// <summary>
    /// Detailed pair information
    /// </summary>
    public class DetailedPairInfo
    {
        /// <summary>
        /// Unificated pair name name {BASE/QUOTE}.
        /// </summary>
        public string UnificatedPairName { get; set; }

        /// <summary>
        /// Base currency.
        /// </summary>
        public string BaseCurrency { get; set; }

        /// <summary>
        /// Quote currency.
        /// </summary>
        public string QuoteCurrency { get; set; }

        /// <summary>
        /// Pair info details
        /// </summary>
        public IEnumerable<PairInfoDetails> Details { get; set; }
    }
}