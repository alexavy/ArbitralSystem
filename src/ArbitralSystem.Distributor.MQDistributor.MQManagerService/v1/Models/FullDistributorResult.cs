using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models
{
    /// <summary>
    /// Result of created orderbook distributor with exchange infos
    /// </summary>
    [UsedImplicitly]
    public class FullDistributorResult : DistributorResult
    {
        /// <summary>
        /// Distributor exchanges info
        /// </summary>
        public IEnumerable<ExchangeInfo> ExchangeInfos { get; set; }
    }

    
    /// <summary>
    /// Distributor result
    /// </summary>
    public class DistributorResult
    {
        /// <summary>
        /// Distributor id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Created at.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Distributor status.
        /// </summary>
        public Status Status { get; set; }
    }
}