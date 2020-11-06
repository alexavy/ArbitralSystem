using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries
{
    /// <summary>
    /// In status Activated or OnProcessing
    /// </summary>
    public class ActiveDistributorByNameQuery  : IRequest<IDistributor>
    {
        public string Name { get; }
        public DistributorType Type { get; }
        
        public ActiveDistributorByNameQuery(string name, DistributorType type)
        {
            Name = name;
            Type = type;
        }
    }
}