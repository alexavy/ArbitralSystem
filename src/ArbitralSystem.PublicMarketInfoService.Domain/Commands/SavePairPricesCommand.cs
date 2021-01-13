using System.Collections.Generic;
using ArbitralSystem.Domain.MarketInfo;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Commands
{
    public class SaveLastPairPricesCommand : IRequest
    {
        public IEnumerable<Exchange> Exchanges { get; }

        public SaveLastPairPricesCommand(IEnumerable<Exchange> exchanges)
        {
            Exchanges = exchanges;
        }
    }
}