using ArbitralSystem.Common.Validation;
using AutoMapper;
using JetBrains.Annotations;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly MarketInfoBdContext DbContext;
        protected readonly IMapper Mapper;
        
        protected BaseRepository([NotNull]MarketInfoBdContext dbContext,[NotNull] IMapper mapper)
        {
            Preconditions.CheckNotNull(dbContext, mapper);
            DbContext = dbContext;
            Mapper = mapper;
        }
    }
}