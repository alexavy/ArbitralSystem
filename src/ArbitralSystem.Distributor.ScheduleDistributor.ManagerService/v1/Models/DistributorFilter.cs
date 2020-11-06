using ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.v1.Models.Paging;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.v1.Models
{
    /// <summary>
    /// Distributor filter
    /// </summary>
    public class DistributorFilter : PageFilter
    {
        /// <summary>
        /// Server name
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// Queue name
        /// </summary>
        public string QueueName { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
    }
}