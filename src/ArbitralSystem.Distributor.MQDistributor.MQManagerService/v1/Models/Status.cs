namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models
{
    /// <summary>
    /// Distributor status enumeration.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Created
        /// </summary>
        Created,
        
        /// <summary>
        /// On processing
        /// </summary>
        Processing,
        
        /// <summary>
        /// On deleting
        /// </summary>
        OnDeleting,
        
        /// <summary>
        /// Deleted
        /// </summary>
        Deleted
    }
}