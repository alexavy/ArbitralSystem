using ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models.Paging;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models
{
    /// <summary>
    /// Distributor filter
    /// </summary>
    public class DistributorFilter : PageFilter
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Status
        /// </summary>
        public Status? Status { get; set; }
        
        /// <summary>
        /// Except status
        /// </summary>
        public Status? ExceptStatus { get; set; }
    }
}