using System;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Mapping.AuxiliaryModels
{
    internal class ExchangeInfo : IExchangeInfo
    {
        public ExchangeInfo(Exchange exchange, DateTimeOffset? heartBeat)
        {
            Exchange = exchange;
            HeartBeat = heartBeat;
        }

        public Exchange Exchange { get;  }
        public DateTimeOffset? HeartBeat { get;  }
    }
}