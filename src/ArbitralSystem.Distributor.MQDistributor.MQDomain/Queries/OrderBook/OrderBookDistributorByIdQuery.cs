using System;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.OrderBook
{
    public class OrderBookDistributorByIdQuery :  IRequest<IOrderBookDistributor>
    {
        public Guid Id { get; }

        public OrderBookDistributorByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}