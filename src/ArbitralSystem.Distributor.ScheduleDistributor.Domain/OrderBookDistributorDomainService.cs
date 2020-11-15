using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Common;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Interfaces;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Jobs;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.OrderBook;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain
{
    public class OrderBookDistributorDomainService
    {
        private readonly IDistributorRepository _distributorRepository;
        private readonly OrderBookDistributorJob _job;
        private readonly IJobManager _jobManager;   
        private readonly IMediator _mediator;
        private readonly string _distributorType = DistributorConstants.OrderBookDistributorIdentity;
        
        public OrderBookDistributorDomainService(IDistributorRepository distributorRepository,OrderBookDistributorJob job, IJobManager jobManager,
            IMediator mediator)
        {
            _distributorRepository = distributorRepository;
            _jobManager = jobManager;
            _mediator = mediator;
            _job = job;
        }
        
        public async Task<string> CreateDistributor(ExchangePairInfo pairInfo, CancellationToken cancellationToken)
        {
            var existedSameDistributor = await _mediator.Send(new DistributorByNameQuery(pairInfo.UnificatedPairName, _distributorType),cancellationToken);
            if (existedSameDistributor != null)
                throw new InvalidOperationException("Distributor with type: {type}, with name: {name}, already exist.");
            
            var servers = (await _mediator.Send(new OrderBookDistributorServerInfoQuery(),cancellationToken)).ToArray();
            if(!servers.Any())
                throw new InvalidOperationException($"No available servers for type: {_distributorType}.");
            
            var existedDistributors = (await  _mediator.Send(new DistributorQueryByType(_distributorType),cancellationToken)).ToArray();
            if(servers.Sum(o=>o.TotalCapacity) <= existedDistributors.Count())
                throw new InvalidOperationException($"There is no free servers to enqueue job");

            var existedQueuesCapacity = existedDistributors.GroupBy(o => o.QueueName).Select(o => new
            {
                QueueName = o.Select(x => x.QueueName).First(),
                Count = o.Count()
            }).OrderByDescending(o => o.Count).ToArray();

           IServerInfo freeServer = null;
            foreach (var server in servers)
            {
                var busyServer = existedQueuesCapacity.FirstOrDefault(o => o.QueueName == server.Queue);

                if (busyServer is null &&  server.TotalCapacity > 0)
                    freeServer = server;
                
                if (busyServer != null && busyServer.Count < server.TotalCapacity)
                    freeServer = server;
            }
            
            if(freeServer is null)
                throw new InvalidOperationException($"There is no free servers to enqueue job");
            
            var distributorPair = new DistributorExchangePairs(freeServer.Queue, freeServer.Name, pairInfo);
            var jobId = _jobManager.Enqueue(distributorPair.QueueName,() => _job.Distribute(distributorPair, new CancellationToken()));

            var result = await _distributorRepository.CreateAsync(
                new Models.Distributor(jobId, pairInfo.UnificatedPairName, _distributorType, freeServer.Name, freeServer.Queue, DateTimeOffset.Now), cancellationToken);
            return result.Id;
        }

        public async Task DeleteDistributor(string name, CancellationToken cancellationToken)
        {
            var existedDistributor = await _mediator.Send(new DistributorByNameQuery(name, _distributorType),cancellationToken);
            if (existedDistributor != null)
            {
                await _distributorRepository.Delete(existedDistributor.Id, cancellationToken);
                _jobManager.Delete(existedDistributor.Id);
            }
        }
    }
}