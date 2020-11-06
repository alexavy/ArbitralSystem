using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.Server
{
    public class ServerQuery : IRequest<Page<IServer>>
    {
        public ServerFilter Filter { get; }

        public ServerQuery(ServerFilter filter)
        {
            Filter = filter;
        }
    }
}