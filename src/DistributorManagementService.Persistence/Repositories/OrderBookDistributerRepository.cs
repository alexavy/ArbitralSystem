using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DistributorManagementService.Domain.Interfaces;
using DistributorManagementService.Domain.Models;
using DistributorManagementService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace DistributorManagementService.Persistence.Repositories
{
    public class OrderBookDistributerRepository: IOrderBookDistributerRepository
    {
        private readonly DistributorDbContext _context;
        private readonly IMapper _mapper;

        public OrderBookDistributerRepository(DistributorDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IOrderBookDistributor> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var distributor = await GetOrderBookDistributorAsync(id, cancellationToken);
            return _mapper.Map<IOrderBookDistributor>(distributor);
        }

        private async Task<Distributor> GetOrderBookDistributorAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Distributors
                .Include(o => o.OrderBookDistributor)
                .ThenInclude(o => o.OrderBookDistributorProperties)
                .FirstOrDefaultAsync(o => o.Id.Equals(id), cancellationToken);
        }
        
        public async Task<IOrderBookDistributor> CreateAsync(IOrderBookDistributor orderBookDistributor, CancellationToken cancellationToken)
        {
            var existedDistributor = await _context.Distributors
                .FirstOrDefaultAsync(o => o.Id.Equals(orderBookDistributor.Id), cancellationToken);
            if (existedDistributor != null)
                throw new InvalidOperationException("Can not create, distributor with same id already exists");

            var distributor = _mapper.Map<Distributor>(orderBookDistributor);
            await _context.Distributors.AddAsync(distributor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return await GetAsync(orderBookDistributor.Id, cancellationToken);
        }

        public async Task<IOrderBookDistributor> UpdateAsync(IOrderBookDistributor orderBookDistributor, CancellationToken cancellationToken)
        {
            var existedDistributor = await GetOrderBookDistributorAsync(orderBookDistributor.Id, cancellationToken);
            if (existedDistributor == null)
                throw new InvalidOperationException("Can not update, distributor not exists");

            existedDistributor.DistributorState = orderBookDistributor.DistributorState;
            existedDistributor.DeletedAt = orderBookDistributor.DeletedAt;
            if (existedDistributor.OrderBookDistributor != null)
            {
                existedDistributor.OrderBookDistributor.Name = orderBookDistributor.Name;
                existedDistributor.OrderBookDistributor.UnificatedPair = orderBookDistributor.UnificatedPair;
                existedDistributor.OrderBookDistributor.OrderBookDistributorProperties.Clear();
                var properties = _mapper.Map<IEnumerable<OrderBookDistributorProperty>>(orderBookDistributor.DistributorProperties);
                properties.ToList().ForEach(o=>existedDistributor.OrderBookDistributor.OrderBookDistributorProperties.Add(o));
            }
            await _context.SaveChangesAsync(cancellationToken);
            return await GetAsync(orderBookDistributor.Id, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var existedDistributor = await _context.Distributors
                .FirstOrDefaultAsync(o => o.Id.Equals(id), cancellationToken);

            if(existedDistributor is null)
                throw new InvalidOperationException("Can not delete distributor, bot not exist.");
            
            _context.Distributors.Remove(existedDistributor);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}