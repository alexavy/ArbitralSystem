using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Common.Pagination.SqlServer;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.Server;
using ArbitralSystem.Distributor.MQDistributor.MQPersistence.Entities;
using ArbitralSystem.Distributor.MQDistributor.MQPersistence.QueryHandlers.Specs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.QueryHandlers
{
    public class ServerQueryHandler : Query, IRequestHandler<ServerQueryById, IServer>,
        IRequestHandler<ServerQuery, Page<IServer>>
    {
        public ServerQueryHandler(DistributorDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IServer> Handle(ServerQueryById request, CancellationToken cancellationToken)
        {
            var server = await DbContext.Servers
                .AsNoTracking()
                .Include(o => o.Distributors)
                .FirstOrDefaultAsync(o => o.Id.Equals(request.ServerId), cancellationToken);

            if (server != null)
            {
                if (request.IsDeletedDistributors.HasValue)
                    server.Distributors = server.Distributors
                        .Where(o => request.IsDeletedDistributors.Value
                            ? o.Status == Status.Deleted
                            : o.Status != Status.Deleted)
                        .ToArray();
            }

            return Mapper.Map<IServer>(server);
        }

        public async Task<Page<IServer>> Handle(ServerQuery request, CancellationToken cancellationToken)
        {
            return await DbContext.Servers
                .AsNoTracking()
                .Include(o => o.Distributors)
                .Where(ServerSpecs.ByType(Mapper.Map<ServerType>(request.Filter.Type)))
                .Where(ServerSpecs.ByIsDeleted(request.Filter.IsDeleted))
                .OrderBy(o => o.Name)
                .Page(request.Filter, Mapper.Map<IServer>, cancellationToken);
        }
    }
}