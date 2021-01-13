using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Common.Pagination.SqlServer;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using ArbitralSystem.Distributor.MQDistributor.MQPersistence.Entities;
using ArbitralSystem.Distributor.MQDistributor.MQPersistence.QueryHandlers.Specs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.QueryHandlers
{
    public class DistributorQueryHandler : Query, IRequestHandler<ActiveDistributorByNameQuery, IDistributor>,
        IRequestHandler<DistributorQuery, Page<IDistributor>>, 
        IRequestHandler<OnProcessingDistributorByTypeQuery, IEnumerable<IDistributor>>
    {
        public DistributorQueryHandler(DistributorDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IDistributor> Handle(ActiveDistributorByNameQuery request, CancellationToken cancellationToken)
        {
            var result = await DbContext.Distributors
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Name.Equals(request.Name) 
                                          && (o.Status != Status.Deleted && o.Status != Status.OnDeleting)
                                          && o.Type == Mapper.Map<DistributorType>(request.Type), cancellationToken);

            return Mapper.Map<IDistributor>(result);
        }

        public async Task<IEnumerable<IDistributor>> Handle(OnProcessingDistributorByTypeQuery request, CancellationToken cancellationToken)
        {
            var result = await DbContext.Distributors
                .Where(o => o.Type == Mapper.Map<DistributorType>(request.Type) && o.Status == Status.Processing )
                .ToArrayAsync(cancellationToken);
            
            return Mapper.Map<IEnumerable<IDistributor>>(result);
        }
        
        public async Task<Page<IDistributor>> Handle(DistributorQuery request, CancellationToken cancellationToken)
        {
            return await DbContext.Distributors
                .AsNoTracking()
                .Where(DistributorSpecs.ByName(request.Filter.Name))
                .Where(DistributorSpecs.ByType(Mapper.Map<DistributorType>(request.Filter.Type)))
                .OrderBy(o => o.Name)
                .Page(request.Filter, Mapper.Map<IDistributor>, cancellationToken);
        }
    }
}