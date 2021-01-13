using System.Threading.Tasks;
using ArbitralSystem.Connectors.Core.Distributers;
using DistributorManagementService.Domain.Models;

namespace DistributorManagementService.Domain.Interfaces
{
    public interface IMessageProducer
    {
        Task RunBotAsync(string botName, IOrderBookDistributor orderbookDistributer);
        Task StopBotAsync(string botName, IOrderBookDistributor orderbookDistributer);
    }
}