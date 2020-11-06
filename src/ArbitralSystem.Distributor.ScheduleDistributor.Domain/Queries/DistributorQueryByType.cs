using System.Collections.Generic;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries
{
    public class DistributorQueryByType : IRequest<IEnumerable<IDistributor>>
    {
        public string Type { get; }

        public DistributorQueryByType( string type)
        {
            Type = type;
        }
    }
}