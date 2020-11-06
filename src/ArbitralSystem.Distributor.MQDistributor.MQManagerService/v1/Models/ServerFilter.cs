using ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models.Paging;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models
{
    /// <summary>
    /// Server filter
    /// </summary>
    public class ServerFilter : PageFilter
    {
        /// <summary>
        /// Server type.
        /// </summary>
        public ServerType? Type { get; set; }
        
        /// <summary>
        /// Is server deleted.
        /// </summary>
        public bool? IsDeleted { get; set; }
    }

    /// <summary>
    /// Server type enumeration.
    /// </summary>
    public enum ServerType
    {
        /// <summary>
        /// order book distributor.
        /// </summary>
        OrderBookDistributor,
    }
}