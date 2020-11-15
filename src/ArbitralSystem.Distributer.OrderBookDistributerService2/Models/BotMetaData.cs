using System;
using ArbitralSystem.Domain.Distributers;

namespace ArbitralSystem.Distributer.OrderBookDistributerService.Models
{
    public class BotMetaData
    {
        public Guid BotId { get; set; }
        public string BotName { get; set; }
        public DistributorState BotState { get; set; }

        public BotMetaData ChangeState(DistributorState state)
        {
            BotState = state;
            return this;
        }
        
    }
}