using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.PublicMarketInfoService.Domain.Models;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Interfaces
{
    public interface IPairPricesRepository
    {
        Task BulkSave(PairPrice[] pairPrices, CancellationToken token);
    }
}