using System.Collections.Generic;
using ArbitralSystem.Domain.MarketInfo;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models
{
    /// <summary>
    /// Args to start new orderbook distributor
    /// </summary>
    public class OrderBookDistributorArg
    {
        /// <summary>
        /// Unificated pair name for distributor
        /// </summary>
        public string UnificatedExchangePairName { get; set; }
        /// <summary>
        /// Exchange constraints
        /// </summary>
        public IEnumerable<Exchange> Exchanges { get; set; }
    }
}