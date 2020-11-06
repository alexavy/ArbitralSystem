namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models
{
    /// <summary>
    /// Order book result of deleting
    /// </summary>
    public class DeletedDistributorResult
    {
        /// <summary>
        /// distributor name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// is order book distributor was deleted
        /// </summary>
        public bool IsDeleted { get; set; }
        
        /// <summary>
        /// if not deleted, error message
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}