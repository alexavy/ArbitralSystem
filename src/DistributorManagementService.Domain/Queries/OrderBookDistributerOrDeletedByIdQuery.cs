using System;
using DistributorManagementService.Domain.Models;
using MediatR;

namespace DistributorManagementService.Domain.Queries
{
    public class OrderBookDistributerOrDeletedByIdQuery : IRequest<IOrderBookDistributor>
    {
        public Guid Id { get; }

        public OrderBookDistributerOrDeletedByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}