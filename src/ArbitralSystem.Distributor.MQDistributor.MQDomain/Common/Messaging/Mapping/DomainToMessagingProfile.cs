using System.Collections.Generic;
using ArbitralSystem.Distributor.Core.Models;
using AutoMapper;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Common.Messaging.Mapping
{
    public class DomainToMessagingProfile : Profile
    {
        public DomainToMessagingProfile()
        {
            CreateMap<PairInfo, ArbitralSystem.Messaging.Models.PairInfo>();
        }
    }
}