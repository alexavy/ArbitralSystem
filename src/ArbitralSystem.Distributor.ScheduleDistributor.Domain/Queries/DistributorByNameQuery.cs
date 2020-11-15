using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries
{
    public class DistributorByNameQuery : IRequest<IDistributor>
    {
        public string Name { get; }
        public string Type { get; }

        public DistributorByNameQuery(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}