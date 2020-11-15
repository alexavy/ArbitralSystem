using ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Entities;
using AutoMapper;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Mapping
{
    public class DomainToPersistence : Profile
    {
        public DomainToPersistence()
        {
            CreateMap<Domain.Models.DistributerState, DistributerState>();
        }
    }
}