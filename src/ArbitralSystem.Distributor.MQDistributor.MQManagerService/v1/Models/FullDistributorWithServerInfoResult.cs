using System;
using JetBrains.Annotations;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models
{
    /// <summary>
    /// Result of created orderbook distributor with server info
    /// </summary>
    [UsedImplicitly]
    public class FullDistributorWithServerInfoResult : FullDistributorResult
    {
        /// <summary>
        /// Server identifier.
        /// </summary>
        public Guid? ServerId { get; set; }
    }
}