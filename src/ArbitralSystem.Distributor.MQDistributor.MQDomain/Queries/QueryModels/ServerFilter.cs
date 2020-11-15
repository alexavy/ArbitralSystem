using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels
{
    public class ServerFilter : PageFilter
    {
        public ServerType? Type { get; set; }
        public bool? IsDeleted { get; set; }
    }
}