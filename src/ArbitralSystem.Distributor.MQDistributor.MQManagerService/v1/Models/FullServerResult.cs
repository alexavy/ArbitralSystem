using System.Collections.Generic;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models
{
    /// <summary>
    /// Full server information, includes distributors info. 
    /// </summary>
    public class FullServerResult : ServerResult
    {
        /// <summary>
        /// Distributors on servers
        /// </summary>
        public IEnumerable<DistributorResult> Distributors { get; set; }
    }
}