using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.Server;
using ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Controllers
{
    /// <inheritdoc />
    [Route("api/v1/server")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public ServerController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Get paginated servers
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ServerFilter filter, CancellationToken cancellationToken)
        {
            var query = new ServerQuery(_mapper.Map<MQDomain.Queries.QueryModels.ServerFilter>(filter));
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(_mapper.Map<Models.Paging.Page<ShortServerResult>>(result));
        }
        
        /// <summary>
        /// Full server info result
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDeletedDistributors"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id,[FromQuery] bool? isDeletedDistributors , CancellationToken cancellationToken)
        {
            var query = new ServerQueryById(id, isDeletedDistributors);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(_mapper.Map<FullServerResult>(result));
        }
    }
}