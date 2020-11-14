
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Commands;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters;
using ArbitralSystem.PublicMarketInfoService.v1.Models;
using ArbitralSystem.PublicMarketInfoService.v1.Models.Paging;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PairInfoFilter = ArbitralSystem.PublicMarketInfoService.v1.Models.PairInfoFilter;
using PolygonFilter = ArbitralSystem.PublicMarketInfoService.v1.Models.PolygonFilter;

namespace ArbitralSystem.PublicMarketInfoService.v1.Controllers
{
    /// <inheritdoc />
    [Route("api/v1/pair-info")]
    [ApiController]
    public class PairInfoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        /// <inheritdoc />
        public PairInfoController(IMapper mapper, IMediator mediator)
        {
            Preconditions.CheckNotNull(mapper, mediator);
            _mapper = mapper;
            _mediator = mediator;
        }

        /// <summary>
        /// Get pairs by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(Page<PairInfo>), 200)]
        public async Task<IActionResult> Get([FromQuery] PairInfoFilter filter, CancellationToken cancellationToken)
        {
            var query = new PairInfoFilterQuery(_mapper.Map<Domain.Queries.Filters.PairInfoFilter>(filter));
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(_mapper.Map<Page<PairInfo>>(result));
        }
        
        /// <summary>
        /// Get unique
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("unique")]
        [ProducesResponseType(typeof(Page<PairInfo>), 200)]
        public async Task<IActionResult> GetUnique([FromQuery] PairInfoFilter filter, CancellationToken cancellationToken)
        {
            var query = new UniquePairInfoFilterQuery(_mapper.Map<Domain.Queries.Filters.PairInfoFilter>(filter));
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(_mapper.Map<Page<UniquePairInfo>>(result));
        }

        /// <summary>
        /// Get detailed pair info
        /// </summary>
        /// <param name="unificatedPair"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("detailed/{unificatedPair}")]
        [ProducesResponseType(typeof(DetailedPairInfo), 200)]
        public async Task<IActionResult> Get([FromRoute] string unificatedPair, CancellationToken cancellationToken)
        {
            var query = new PairInfosByUnificatedNameQuery( unificatedPair?.Replace('-','/') );
            var result = (await _mediator.Send(query, cancellationToken)).ToArray();

            if (!result.Any())
                return NoContent();
            
            var basePairInfo = result.First();
            return Ok(new DetailedPairInfo()
            {
                UnificatedPairName = basePairInfo.UnificatedPairName,
                BaseCurrency = basePairInfo.BaseCurrency,
                QuoteCurrency = basePairInfo.QuoteCurrency,
                Details = result.Select(o=> new PairInfoDetails()
                {
                    ExchangePairName = o.ExchangePairName,
                    CreatedAt = o.CreatedAt,
                    DelistedAt = o.DelistedAt,
                    Exchange = o.Exchange
                })
            });
        }
            
        /// <summary>
        /// Get all pairs for exchange
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{exchange}")]
        [ProducesResponseType(typeof(IEnumerable<PairInfo>), 200)]
        public async Task<IActionResult> Get([FromRoute] Exchange exchange, CancellationToken cancellationToken)
        {
            var query = new PairInfoByExchangeQuery(exchange, true);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(_mapper.Map<IEnumerable<PairInfo>>(result));
        }
        
        /// <summary>
        /// Find chained pairs, triangles.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("triangles/{exchange}")]
        public async Task<IActionResult> Get([FromRoute] Exchange exchange,[FromQuery] PolygonFilter filter ,CancellationToken cancellationToken)
        {
            if (exchange == Exchange.Undefined)
                return BadRequest("Exchange not available.");
            
            var pairs = await _mediator.Send(new PairInfoByExchangeQuery(exchange, true), cancellationToken);
            var command = new FindTrianglesCommand(pairs, _mapper.Map<Domain.Queries.Filters.PolygonFilter>(filter));
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(_mapper.Map<IEnumerable<PairInfoPolygon>>(result).Select(o=>o.UnificatedPairs));
        }
    }
}