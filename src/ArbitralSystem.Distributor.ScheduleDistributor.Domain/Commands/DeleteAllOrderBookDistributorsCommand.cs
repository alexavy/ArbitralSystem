using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Common;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Commands
{
    public class DeleteAllOrderBookDistributorsCommand : BaseOrderBookDistributorRequest, IRequest
    {
    }
}