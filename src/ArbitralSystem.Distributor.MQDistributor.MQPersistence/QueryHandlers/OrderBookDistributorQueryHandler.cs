using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Common.Pagination.SqlServer;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.OrderBook;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using ArbitralSystem.Distributor.MQDistributor.MQPersistence.Entities;
using ArbitralSystem.Distributor.MQDistributor.MQPersistence.QueryHandlers.Specs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.QueryHandlers
{
    public class OrderBookDistributorQueryHandler : Query , IRequestHandler<OrderBookDistributorByIdQuery, IOrderBookDistributor>,
        IRequestHandler<OrderBookDistributorQuery, Page<IOrderBookDistributor>>
    {
        public OrderBookDistributorQueryHandler(DistributorDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IOrderBookDistributor> Handle(OrderBookDistributorByIdQuery request, CancellationToken cancellationToken)
        {
            var dist = await DbContext.Distributors
                .AsNoTracking()
                .Include(o=>o.Server)
                .Include(o=>o.Exchanges).ThenInclude(o=>o.Exchange)
                .FirstOrDefaultAsync(o => o.Id.Equals(request.Id),cancellationToken);

            return Mapper.Map<IOrderBookDistributor>(dist);
        }
        
        public async Task<Page<IOrderBookDistributor>> Handle(OrderBookDistributorQuery request, CancellationToken cancellationToken)
        {
            return await DbContext.Distributors
                .AsNoTracking()
                .Include(o=>o.Server)
                .Include(o=>o.Exchanges).ThenInclude(o=>o.Exchange)
                .Where(DistributorSpecs.ByName(request.Filter.Name))
                .Where(DistributorSpecs.ByStatus(Mapper.Map<Status?>(request.Filter.Status)))
                .Where(DistributorSpecs.ByExceptStatus(Mapper.Map<Status?>(request.Filter.ExceptStatus)))
                .Where(DistributorSpecs.ByType(Mapper.Map<DistributorType>(request.Filter.Type)))
                .OrderBy(o=>o.Name)
                .Page(request.Filter,Mapper.Map<IOrderBookDistributor>, cancellationToken);
        }
    }
}