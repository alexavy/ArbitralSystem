using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using JetBrains.Annotations;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries
{
    public class DistributorQuery  : IRequest<Page<IDistributor>>
    {
        public DistributorFilter Filter { get; }
        
        public DistributorQuery(DistributorFilter filter)
        {
            Filter = filter;
        }
    }
}