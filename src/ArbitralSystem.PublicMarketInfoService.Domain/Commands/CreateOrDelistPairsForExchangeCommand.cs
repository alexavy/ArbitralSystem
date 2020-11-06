using ArbitralSystem.Domain.MarketInfo;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Commands
{
    public class CreateOrDelistPairsForExchangeCommand : IRequest
    {
        public CreateOrDelistPairsForExchangeCommand(Exchange exchange)
        {
            Exchange = exchange;
        }

        public Exchange Exchange { get; }
    }
}