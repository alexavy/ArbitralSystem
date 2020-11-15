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

        public async Task<DistributerState> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var distributerState = await DbContext.DistributerStates.FirstOrDefaultAsync(o => o.Id.Equals(id), cancellationToken);
            return Mapper.Map<DistributerState>(distributerState);
        }
        
        public async Task<DistributerState> SaveAsync(DistributerState distributerState, CancellationToken cancellationToken)
        {
            var existedState = await GetAsync(distributerState.Id, cancellationToken);
            if (existedState != null) 
                throw new InvalidOperationException($"Can not create DistributerState .PairInfo with id {distributerState.Id} already exist");
            
            DbContext.DistributerStates.Add(Mapper.Map<Entities.DistributerState>(distributerState));
            await DbContext.SaveChangesAsync(cancellationToken);
            return Mapper.Map<DistributerState>(await GetAsync(distributerState.Id, cancellationToken));
        }
    }
}