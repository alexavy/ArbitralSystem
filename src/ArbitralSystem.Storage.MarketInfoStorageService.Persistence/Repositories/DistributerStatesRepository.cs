using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Interfaces;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Repositories
{
    public class DistributerStatesRepository : BaseRepository , IDistributerStatesRepository
    {
        public DistributerStatesRepository([NotNull] MarketInfoBdContext dbContext, [NotNull] IMapper mapper) : base(dbContext, mapper)
        {
        }
        
        public async Task SaveWithNoCheckAsync(DistributerState distributerState, CancellationToken cancellationToken)
        {
            DbContext.DistributerStates.Add(Mapper.Map<Entities.DistributorState>(distributerState));
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}