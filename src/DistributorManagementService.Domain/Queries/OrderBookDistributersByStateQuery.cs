using System.Collections;
using System.Collections.Generic;
using ArbitralSystem.Domain.Distributers;
using DistributorManagementService.Domain.Models;
using MediatR;

namespace DistributorManagementService.Domain.Queries
{
    public class OrderBookDistributersByStateQuery : IRequest<IEnumerable<IOrderBookDistributor>>
    {
        public DistributorState DistributorState { get; }

        public OrderBookDistributersByStateQuery(DistributorState state)
        {
            DistributorState = state;
        }
    }
}