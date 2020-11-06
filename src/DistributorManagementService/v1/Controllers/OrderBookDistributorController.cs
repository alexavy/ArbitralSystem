using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Pagination;
using AutoMapper;
using DistributorManagementService.v1.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DistributorManagementService.v1.Controllers
{
    /// <inheritdoc />
    [Route("api/v1/orderbook-distributor")]
    [ApiController]
    public class OrderBookDistributorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="mediator"></param>
        public OrderBookDistributorController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        /// <summary>
        /// Get all distributors by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(Page<OrderBookDistributor>), 200)]
        public async Task<IActionResult> Get([FromQuery]OrderBookDistributorFilter filter, CancellationToken cancellationToken)
        {
            return Ok();
        }
        
        [HttpPost("run/{pair}")]
        [ProducesResponseType(typeof(Page<OrderBookDistributor>), 200)]
        public async Task<IActionResult> Run([FromRoute]string pair, CancellationToken cancellationToken)
        {
            return Ok();
        }
        
        [HttpPut("stop/{id}")]
        [ProducesResponseType(typeof(Page<OrderBookDistributor>), 200)]
        public async Task<IActionResult> Stop([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        
    }
}