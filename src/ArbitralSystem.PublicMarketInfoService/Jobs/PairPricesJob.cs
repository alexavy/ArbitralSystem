using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Commands;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Jobs
{
    [UsedImplicitly]
    internal class PairPricesJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public PairPricesJob(ILogger logger, IMediator mediator)
        {
            Preconditions.CheckNotNull(mediator, logger);
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Execute()
        {
            await _mediator.Send(new SaveLastPairPricesCommand(ExchangeHelper.GetAll()));
        }
    }
}