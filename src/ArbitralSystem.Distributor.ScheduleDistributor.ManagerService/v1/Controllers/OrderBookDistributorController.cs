using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Connectors.ArbitralPublicMarketInfoConnector.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Commands;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.OrderBook;
using ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Services;
using ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.v1.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.v1.Controllers
{
    /// <inheritdoc />
    [Route("api/v1/orderbook-distributor")]
    [ApiController]
    public class OrderBookDistributorController : ControllerBase
    {
        private readonly PairInfoService _pairInfoService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        
        /// <inheritdoc />
        public OrderBookDistributorController(
            PairInfoService pairInfoService,
            IMediator mediator,
            IMapper mapper)
        {
            _mapper = mapper;
            _mediator = mediator;
            _pairInfoService = pairInfoService;
        }
        
        /// <summary>
        /// Get order book distributors by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]DistributorFilter filter, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new OrderBookDistributorQuery(_mapper.Map<ScheduleDistributor.Domain.Queries.QueryModels.DistributorFilter>(filter)),
                cancellationToken);
            return Ok(_mapper.Map<Models.Paging.Page<OrderBookDistributorResult>>(result));
        }
        
        
        /// <summary>
        /// Creates orderbook distributor
        /// </summary>
        /// <param name="distributorArg"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Post([FromBody] OrderBookDistributorArg distributorArg, CancellationToken cancellationToken)
        {
            var availablePairs = (await _pairInfoService
                .GetPairInfo(distributorArg.UnificatedExchangePairName, distributorArg.Exchanges?.ToArray()));
            
            var distributorId = await _mediator.Send(new CreateOrderBookDistributorCommand(new ExchangePairInfo(availablePairs.ToArray())), cancellationToken);
            var result = await _mediator.Send(new OrderBookDistributorByIdQuery(distributorId),cancellationToken);
            return Ok(_mapper.Map<OrderBookDistributorResult>(result));
        }
        

        /// <summary>
        /// Delete bot by pair name
        /// </summary>
        /// <param name="unificatedPair"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{unificatedPair}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete([FromRoute]string unificatedPair, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteOrderBookDistributorCommand(unificatedPair.Replace('-','/')), cancellationToken);
            return Ok();
        }
        
        /// <summary>
        /// Delete all active bots
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("all")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete(CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteAllOrderBookDistributorsCommand(), cancellationToken);
            return Ok();
        }
    }
}