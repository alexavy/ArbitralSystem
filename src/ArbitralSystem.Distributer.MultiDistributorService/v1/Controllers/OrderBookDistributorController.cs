using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Distributer.OrderBookMultiDistributorDomain.Services;
using ArbitralSystem.Distributer.OrderBookMultiDistributorService.v1.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArbitralSystem.Distributer.OrderBookMultiDistributorService.v1.Controllers
{
    [ApiController]
    [Route("api/v1/multi-distributor")]
    public class OrderBookDistributorController : ControllerBase
    {
        private readonly DistributorManagerDomainService _domainService;
        
        public OrderBookDistributorController(DistributorManagerDomainService domainService)
        {
            _domainService = domainService;
        }
        
        [HttpPost("{exchange}")]
        public async Task<IActionResult> Post([FromBody] OrderBookDistributorData distributorData, CancellationToken cancellationToken)
        {
            await _domainService.CreateNewDistributor(distributorData.BaseCurrency, distributorData.QuoteCurrency);
            return Ok();
        }
    }
}