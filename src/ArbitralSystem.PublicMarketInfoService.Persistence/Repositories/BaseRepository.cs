using System.Diagnostics.CodeAnalysis;
using ArbitralSystem.Common.Validation;
using AutoMapper;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly PublicMarketInfoBdContext DbContext;
        protected readonly IMapper Mapper;

        protected BaseRepository([NotNull] PublicMarketInfoBdContext dbContext, [NotNull] IMapper mapper)
        {
            Preconditions.CheckNotNull(dbContext, mapper);
            DbContext = dbContext;
            Mapper = mapper;
        }
    }
}