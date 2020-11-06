using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Commands;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Services
{
    /// <summary>
    ///  PairInfo update service
    /// </summary>
    public class PairInfoUpdaterService
    {
        private readonly IMediator _mediator;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>

        public PairInfoUpdaterService(IMediator mediator)
        {
            Preconditions.CheckNotNull(mediator);
            _mediator = mediator;
        }

        public async Task Update(CancellationToken cancellationToken)
        {
            foreach (var exchange in ExchangeHelper.GetAll())
            {
                var command = new CreateOrDelistPairsForExchangeCommand(exchange);
                await _mediator.Send(command, cancellationToken);
            }
        }
        
        public async Task Update(Exchange exchange, CancellationToken cancellationToken)
        {
            if(exchange == Exchange.Undefined)
                throw new ArgumentException("Exchange cannot be Undefined");
            
            var command = new CreateOrDelistPairsForExchangeCommand(exchange);
            await _mediator.Send(command, cancellationToken);
        }
    }
}