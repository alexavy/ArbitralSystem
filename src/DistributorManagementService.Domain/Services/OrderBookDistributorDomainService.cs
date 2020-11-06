using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Domain.Distributers;
using AutoMapper;
using DistributorManagementService.Domain.Exceptions;
using DistributorManagementService.Domain.Interfaces;
using DistributorManagementService.Domain.Models;
using DistributorManagementService.Domain.Queries;
using MediatR;

namespace DistributorManagementService.Domain.Services
{
    public class OrderBookDistributorDomainService
    {
        private readonly IOrderBookDistributerRepository _orderBookDistributerRepository;
        private readonly PairInfoService _pairInfoService;
        private readonly IMessageProducer _producer;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        
        
        public OrderBookDistributorDomainService(IOrderBookDistributerRepository orderBookDistributerRepository,IMessageProducer producer)
        {
            _orderBookDistributerRepository = orderBookDistributerRepository;
            _producer = producer;
        }

        public async Task<IDistributor> CreateBotAsync(Guid id,string name , CancellationToken cancellationToken)
        {
            return await _orderBookDistributerRepository.CreateAsync(new OrderBookDistributor(id,name), cancellationToken);
        }
        
        public async Task<IOrderBookDistributor> RunBotAsync(string unificatedPair,CancellationToken cancellationToken)
        {
            var listeningDistributors = await _mediator.Send(new OrderBookDistributersByStateQuery(DistributorState.Listening),cancellationToken);

            if (!listeningDistributors.Any())
                throw new InvalidDistributorOperation("No available bots in state listening");

            var rawDistributor = await PrepareDistributors(unificatedPair, cancellationToken);
            
            rawDistributor.ChangeState(DistributorState.WaitingForDistribution);
            var updatedDistributor =  await _orderBookDistributerRepository.UpdateAsync(rawDistributor, cancellationToken);
            await _producer.RunBotAsync(updatedDistributor.Name,updatedDistributor);
            return updatedDistributor;
        }

        public async Task StopBotAsync(Guid id,CancellationToken cancellationToken)
        {
            var existedOrderBookDistributor = await _orderBookDistributerRepository.GetAsync(id, cancellationToken);
            
            if (existedOrderBookDistributor is null)
                throw new InvalidOperationException($"Bot not exist with id: {id}");
            
            if (existedOrderBookDistributor.DistributorState != DistributorState.Distribution)
                throw new InvalidDistributorOperation("Can stop bot only in distribution state");
            
            existedOrderBookDistributor.ChangeState(DistributorState.WaitingForStopping);
            await _orderBookDistributerRepository.UpdateAsync(existedOrderBookDistributor,cancellationToken);
            await _producer.StopBotAsync(existedOrderBookDistributor.Name,existedOrderBookDistributor);
        }

        private async Task<IOrderBookDistributor> PrepareDistributors(string unificatedPairs,CancellationToken cancellationToken )
        {
            var distributor = await _mediator.Send(new AnyListeningOrderBookDistributerQuery(),cancellationToken);
            var exchangeAndPairs =  await _pairInfoService.FindExchangesForPair(unificatedPairs);
            distributor.AppointPair(unificatedPairs, exchangeAndPairs.Select(o => new DistributorExchangeProperty(o.ExchangePair, o.Exchange)).ToArray());
            return distributor;
        }
        
        public async Task<IOrderBookDistributor> UpdateStateAsync(Guid id, DistributorState state, CancellationToken cancellationToken )
        {
            var distributor = await _mediator.Send(new OrderBookDistributerOrDeletedByIdQuery(id),cancellationToken);
            if(distributor is null || distributor.DeletedAt != null)
                throw new InvalidDistributorOperation("Can update bot state, already deleted or not created");
            
            distributor.ChangeState(state);
            return await _orderBookDistributerRepository.UpdateAsync(distributor,cancellationToken);
        }
        
        public async Task DeleteBotAsync(Guid id, CancellationToken cancellationToken)
        {
            await _orderBookDistributerRepository.DeleteAsync(id, cancellationToken);
        }
    }
}