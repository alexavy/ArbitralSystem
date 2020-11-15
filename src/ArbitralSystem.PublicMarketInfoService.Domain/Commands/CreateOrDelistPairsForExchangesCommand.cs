using System.Collections.Generic;
using ArbitralSystem.Domain.MarketInfo;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Commands
{
    public class CreateOrDelistPairsForExchangesCommand : IRequest
    {
        public CreateOrDelistPairsForExchangesCommand(IEnumerable<Exchange> exchanges)
        {
            Exchanges = exchanges;
        }

        public IEnumerable<Exchange> Exchanges { get; }
    }
}