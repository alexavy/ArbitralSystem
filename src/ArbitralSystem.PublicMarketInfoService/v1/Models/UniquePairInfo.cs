namespace ArbitralSystem.PublicMarketInfoService.v1.Models
{
    /// <summary>
    /// Unique pair info
    /// </summary>
    public class UniquePairInfo
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
        /// Number of occurrences on exchanges
        /// </summary>
        public int OccurrencesCount { get; set; }
    }
}