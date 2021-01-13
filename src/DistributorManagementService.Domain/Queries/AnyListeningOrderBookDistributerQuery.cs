using DistributorManagementService.Domain.Models;
using MediatR;

namespace DistributorManagementService.Domain.Queries
{
    public class AnyListeningOrderBookDistributerQuery :  IRequest<IOrderBookDistributor>
    {
    }
}