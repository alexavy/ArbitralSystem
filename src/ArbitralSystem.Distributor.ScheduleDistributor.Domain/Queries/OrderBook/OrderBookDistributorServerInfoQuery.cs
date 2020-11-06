using System.Collections.Generic;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Common;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.OrderBook
{
    public class OrderBookDistributorServerInfoQuery : BaseOrderBookDistributorRequest, IRequest<IEnumerable<IServerInfo>>
    {
    }
}