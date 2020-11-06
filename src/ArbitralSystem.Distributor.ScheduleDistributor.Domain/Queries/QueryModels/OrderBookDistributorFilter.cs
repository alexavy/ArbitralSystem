using ArbitralSystem.Common.Pagination;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels
{
    public class DistributorFilter : PageFilter
    {
        public string ServerName { get; set; }
        public string QueueName { get; set; }
        public string Name { get; set; }
    }
}