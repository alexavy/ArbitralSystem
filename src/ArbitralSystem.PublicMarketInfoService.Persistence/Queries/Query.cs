using System;
using AutoMapper;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Queries
{
    public abstract class Query
    {
        protected readonly PublicMarketInfoBdContext DbContext;
        protected readonly IMapper Mapper;

        protected Query(PublicMarketInfoBdContext dbContext, IMapper mapper)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
    }
}