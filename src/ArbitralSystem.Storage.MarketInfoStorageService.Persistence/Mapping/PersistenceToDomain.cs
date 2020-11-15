using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models;
using AutoMapper;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Mapping
{
    public class PersistenceToDomain : Profile
    {
        public PersistenceToDomain()
        {
            CreateMap<Entities.DistributerState, DistributerState>()
                .ConstructUsing(o => new DistributerState(o.Id, o.Symbol, o.Exchange, o.ChangedAt, o.PreviousStatus, o.CurrentStatus));
        }
    }
}