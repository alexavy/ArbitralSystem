using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DistributorManagementService.Domain.Models;
using DistributorManagementService.Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributorManagementService.Persistence.Queries
{
    public class OrderBookDistributersByStateQueryHandler :  Query, IRequestHandler<OrderBookDistributersByStateQuery, IEnumerable< IOrderBookDistributor>>
    {
        public OrderBookDistributersByStateQueryHandler(DistributorDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IEnumerable<IOrderBookDistributor>> Handle(OrderBookDistributersByStateQuery request, CancellationToken cancellationToken)
        {
            var distributors = await DbContext.Distributors
                .AsNoTracking()
                .Include(o => o.OrderBookDistributor).ThenInclude(o => o.OrderBookDistributorProperties)
                .Where(o => o.DistributorState == request.DistributorState && o.DeletedAt.Equals(null))
                .ToArrayAsync(cancellationToken);

            return Mapper.Map<IEnumerable<IOrderBookDistributor>>(distributors);
        }
    }
}