using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.v1.Models.Paging;

namespace ArbitralSystem.PublicMarketInfoService.v1.Models
{
    /// <summary>
    /// Pair filter
    /// </summary>
    public class PairInfoFilter : PageFilter
    {
        /// <summary>
        /// By base currency
        /// </summary>
        public string BaseCurrency { get; set; }
        
        /// <summary>
        /// By quote currency
        /// </summary>
        public string QuoteCurrency { get; set; }
        
        /// <summary>
        /// By Exchange
        /// </summary>
        public Exchange? Exchange { get; set; }
        
        /// <summary>
        /// Dilisted
        /// </summary>
        public bool? IsDelisted { get; set; }
        
        /// <summary>
        /// Pair which listed on more than N exchanges
        /// </summary>
        public int? ListedMoreThan { get; set; }
    }
}