using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries
{
    public class DistributorQueryById : IRequest<IDistributor>
    {
        public string Id { get; }

        public DistributorQueryById(string id)
        {
            Id = id;
        }
    }
}