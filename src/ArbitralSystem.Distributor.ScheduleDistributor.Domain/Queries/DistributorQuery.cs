using System.Collections.Generic;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries
{
    public class DistributorQuery : IRequest<IEnumerable<IDistributor>>
    {
        public string Type { get; }

        public DistributorQuery(string type = null)
        {
            Type = type;
        }
    }
}