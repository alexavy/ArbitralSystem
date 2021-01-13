using System.Threading.Tasks;
using ArbitralSystem.Distributer.OrderBookDistributerService.Models;

namespace ArbitralSystem.Distributer.OrderBookDistributerService.Producers
{

    internal interface IDistributerManagerService
    {
        Task Notify(BotMetaData metaData);
    }
    
    internal class DistributerManagerService
    {
        public DistributerManagerService()
        {
            
        }

        public async Task Notify(BotMetaData metaData)
        {
            
        }
    }
}