using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Common.Exceptions;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters;
using ArbitralSystem.PublicMarketInfoService.v1.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArbitralSystem.PublicMarketInfoService.v1.Controllers
{
    /// <inheritdoc />
    [Route("api/v1/pair-price")]
    [ApiController]
    public class PairPricesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        /// <inheritdoc />
        public PairPricesController(IMapper mapper, IMediator mediator)
        {
            Preconditions.CheckNotNull(mapper, mediator);
            _mapper = mapper;
            _mediator = mediator;
        }

        /// <summary>
        /// Get price data 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="pair"></param>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{exchange}/{pair}")]
        [ProducesResponseType(typeof(Page<PairPrice>), 200)]
        public async Task<IActionResult> Get([FromRoute] Exchange exchange, [FromRoute] string pair,
            [FromQuery] PairPriceFilter filter, CancellationToken cancellationToken)
        {
            if (exchange.Equals(Exchange.Undefined))
                return BadRequest("Exchange can't be Undefined.");

            if (!pair.Contains('-'))
                return BadRequest("Pair should be {base}-{quot}.");

            var result = await _mediator.Send(new PairPriceQuery(FormatPair(pair), exchange, _mapper.Map<IntervalPageFilter>(filter)), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get summary
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="pair"></param>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{exchange}/{pair}/summary")]
        [ProducesResponseType(typeof(PairPriceSummaryInfo), 200)]
        public async Task<IActionResult> GetSummary([FromRoute] Exchange exchange, [FromRoute] string pair,
            [FromQuery] SummaryPairPriceFilter filter, CancellationToken cancellationToken)
        {
            if (exchange.Equals(Exchange.Undefined))
                return BadRequest("Exchange can't be Undefined.");

            if (!pair.Contains('-'))
                return BadRequest("Pair should be {base}-{quot}.");

            if (!filter.From.HasValue && !filter.To.HasValue)
                filter = GetCurrentDayDiapasonFilter();
            
            try
            {
                var result = await _mediator.Send(new SummaryPairPriceQuery(FormatPair(pair), exchange, _mapper.Map<IntervalFilter>(filter)),
                    cancellationToken);
                return Ok(result);
            }
            catch (NoDataForPeriodException)
            {
                return NoContent();
            }
        }

        private static SummaryPairPriceFilter GetCurrentDayDiapasonFilter()
        {
            return new SummaryPairPriceFilter()
            {
                From = new DateTimeOffset(DateTime.Today).AddHours(0),
                To = new DateTimeOffset(DateTime.Today).AddDays(1).AddMilliseconds(-1)
            };
        }
        
        private static string FormatPair(string pair)
        {
            return pair.Replace('-', '/');
        }
    }
}