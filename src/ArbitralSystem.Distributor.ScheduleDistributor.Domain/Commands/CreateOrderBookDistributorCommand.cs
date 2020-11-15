using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Common;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Commands
{
    public class CreateOrderBookDistributorCommand :  IRequest<string>
    {
        public ExchangePairInfo ExchangePairInfo { get; }
        
        public CreateOrderBookDistributorCommand(ExchangePairInfo exchangePairInfo)
        {
            ExchangePairInfo = exchangePairInfo;
        }
    }
}