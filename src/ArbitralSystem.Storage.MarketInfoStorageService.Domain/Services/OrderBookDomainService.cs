using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Interfaces;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Domain.Services
{
    public class OrderBookDomainService
    {
        private readonly IOrderBooksRepository _orderBooksRepository;
        public OrderBookDomainService(IOrderBooksRepository orderBooksRepository)
        {
            _orderBooksRepository = orderBooksRepository;
        }
        
        public async Task BulkSave(OrderBook[] orderBooks, CancellationToken cancellationToken)
        {
            await _orderBooksRepository.BulkSaveAsync(orderBooks, cancellationToken);
        }
    }
}