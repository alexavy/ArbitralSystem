using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels
{
    public  class DistributorFilter : PageFilter
    {
        public string Name { get; set; }
        public Status? Status { get; set; }
        public Status? ExceptStatus { get; set; }
        public virtual DistributorType? Type { get; set; }
    }
}