using System;
using ArbitralSystem.Domain.MarketInfo;
using JetBrains.Annotations;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models
{
    /// <summary>
    /// Exchange extra information
    /// </summary>
    [UsedImplicitly]
    public class ExchangeInfo
    {
        /// <summary>
        /// Exchange
        /// </summary>
        public Exchange Exchange { get; set; }
        
        /// <summary>
        /// Last activity in current exchange
        /// </summary>
        public DateTimeOffset? HeartBeat { get; set; }
    }
}