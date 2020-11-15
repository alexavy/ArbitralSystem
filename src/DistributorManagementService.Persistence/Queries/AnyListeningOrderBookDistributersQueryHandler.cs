using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Domain.Distributers;
using AutoMapper;
using DistributorManagementService.Domain.Models;
using DistributorManagementService.Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributorManagementService.Persistence.Queries
{
    public class AnyListeningOrderBookDistributersQueryHandler : Query, IRequestHandler<AnyListeningOrderBookDistributerQuery, IOrderBookDistributor>
    {
        public AnyListeningOrderBookDistributersQueryHandler(DistributorDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IOrderBookDistributor> Handle(AnyListeningOrderBookDistributerQuery request, CancellationToken cancellationToken)
        {
            var distributors = await DbContext.Distributors.AsNoTracking()
                .Include(o => o.OrderBookDistributor).ThenInclude(o => o.OrderBookDistributorProperties)
                .FirstOrDefaultAsync(o => o.DistributorState == DistributorState.Listening && 
                    o.DeletedAt.Equals(null), cancellationToken);

            return Mapper.Map<IOrderBookDistributor>(distributors);
        }
    }
}