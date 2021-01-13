using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Distributor.Core.Models;
using ArbitralSystem.Distributor.Core.Services;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.OrderBook;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DistributorFilter = ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models.DistributorFilter;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Controllers
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
            _pairInfoService = pairInfoService;
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get order book distributor by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new OrderBookDistributorByIdQuery(id), cancellationToken);
            return Ok(_mapper.Map<FullDistributorWithServerInfoResult>(result));
        }
        
        /// <summary>
        /// Get order book distributors by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DistributorFilter filter, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new OrderBookDistributorQuery(_mapper.Map<OrderBookDistributorFilter>(filter)),
                cancellationToken);
            return Ok(_mapper.Map<Models.Paging.Page<FullDistributorWithServerInfoResult>>(result));
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
            IOrderBookDistributor result = await _mediator.Send(new OrderBookDistributorByIdQuery(distributorId), cancellationToken);
            return Ok(_mapper.Map<FullDistributorResult>(result));
        }

        /// <summary>
        /// This method is auxiliary. This behavior should be executed on front-end. 
        /// </summary>
        /// <param name="distributorArgs"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("several")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Post([FromBody] OrderBookDistributorArg[] distributorArgs, CancellationToken cancellationToken)
        {
            foreach (var distributorArg in distributorArgs)
            {
                var availablePairs = (await _pairInfoService
                    .GetPairInfo(distributorArg.UnificatedExchangePairName, distributorArg.Exchanges?.ToArray()));

                var distributorId = await _mediator.Send(new CreateOrderBookDistributorCommand(new ExchangePairInfo(availablePairs.ToArray())),
                    cancellationToken);
                IOrderBookDistributor result = await _mediator.Send(new OrderBookDistributorByIdQuery(distributorId), cancellationToken);
            }

            return Ok();
        }


        /// <summary>
        /// Delete bot by pair name
        /// </summary>
        /// <param name="unificatedPair"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{unificatedPair}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete([FromRoute] string unificatedPair, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteOrderBookDistributorByNameCommand(unificatedPair.Replace('-', '/')), cancellationToken);
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
            var distributors = await _mediator.Send(new OnProcessingDistributorByTypeQuery(DistributorType.OrderBooks), cancellationToken);
            var deletedDistributors = new List<DeletedDistributorResult>();
            foreach (var distributor in distributors)
            {
                var result = await TryDeleteOrderBookDistributor(distributor.Id, cancellationToken);
                deletedDistributors.Add(new DeletedDistributorResult()
                {
                    Name = distributor.Name,
                    IsDeleted = result.IsSuccess,
                    ErrorMessage = result.ErrorMessage
                });
            }

            return Ok(deletedDistributors);
        }

        private async Task<(bool IsSuccess, string ErrorMessage)> TryDeleteOrderBookDistributor(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new DeleteOrderBookDistributorCommand(id), cancellationToken);
                return (IsSuccess: true, ErrorMessage: null);
            }
            catch (Exception e)
            {
                return (IsSuccess: false, ErrorMessage: e.Message);
            }
        }
    }
}