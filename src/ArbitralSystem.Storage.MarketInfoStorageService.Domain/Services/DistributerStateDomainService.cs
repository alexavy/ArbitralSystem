using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Interfaces;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Domain.Services
{
    public class DistributerStateDomainService
    {
        private readonly IDistributerStatesRepository _distributerStatesRepository;
        public DistributerStateDomainService(IDistributerStatesRepository distributerStatesRepository)
        {
            _distributerStatesRepository = distributerStatesRepository;
        }

        public async Task<DistributerState> SaveState(DistributerState state, CancellationToken cancellationToken)
        {
            return await _distributerStatesRepository.SaveAsync(state,cancellationToken);
        }
    }
}