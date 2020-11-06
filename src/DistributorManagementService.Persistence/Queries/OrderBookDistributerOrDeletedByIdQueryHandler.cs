using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DistributorManagementService.Domain.Models;
using DistributorManagementService.Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributorManagementService.Persistence.Queries
{
    public class OrderBookDistributerOrDeletedByIdQueryHandler : Query, IRequestHandler<OrderBookDistributerOrDeletedByIdQuery, IOrderBookDistributor>
    {
        public OrderBookDistributerOrDeletedByIdQueryHandler(DistributorDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IOrderBookDistributor> Handle(OrderBookDistributerOrDeletedByIdQuery request, CancellationToken cancellationToken)
        {
            var distributor = await DbContext.Distributors
                .AsNoTracking()
                .Include(o => o.OrderBookDistributor).ThenInclude(o => o.OrderBookDistributorProperties)
                .FirstOrDefaultAsync(o => o.Id.Equals(request.Id), cancellationToken);

            return Mapper.Map<IOrderBookDistributor>(distributor);
        }
    }
}