using System.Collections.Generic;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries
{
    public class OnProcessingDistributorByTypeQuery  : IRequest<IEnumerable<IDistributor>>
    {
        public DistributorType Type { get; }
        
        public OnProcessingDistributorByTypeQuery(DistributorType type)
        {
            Type = type;
        }
    }
}