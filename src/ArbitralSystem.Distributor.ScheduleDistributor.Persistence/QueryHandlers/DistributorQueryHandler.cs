using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using ArbitralSystem.Distributor.ScheduleDistributor.Persistence.QueryHandlers.Specs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.QueryHandlers
{
    public class DistributorQueryHandler : Query , IRequestHandler<DistributorByNameQuery, IDistributor>,
        IRequestHandler<DistributorQueryByType, IEnumerable<IDistributor>> , IRequestHandler<DistributorQuery, IEnumerable<IDistributor>>
    {
        public DistributorQueryHandler(DistributorDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IDistributor> Handle(DistributorByNameQuery request, CancellationToken cancellationToken)
        {
            var result = await DbContext.Distributors
                .AsNoTracking().FirstOrDefaultAsync(o => o.Name.Equals(request.Name)
                                                         && o.DistributorType.Equals(request.Type), cancellationToken);

            return Mapper.Map<IDistributor>(result);
        }

        public async Task<IEnumerable<IDistributor>> Handle(DistributorQueryByType request, CancellationToken cancellationToken)
        {
            var result = await DbContext.Distributors
                .AsNoTracking().Where(o => o.DistributorType.Equals(request.Type))
                .ToArrayAsync(cancellationToken);
            
            return Mapper.Map<IEnumerable<IDistributor>>(result);
        }

        public async Task<IEnumerable<IDistributor>> Handle(DistributorQuery request, CancellationToken cancellationToken)
        {
            var result = await DbContext.Distributors
                .AsNoTracking()
                .Where(DistributorSpecs.ByType(request.Type))
                .ToArrayAsync(cancellationToken);
            
            return Mapper.Map<IEnumerable<IDistributor>>(result);
        }
    }
}