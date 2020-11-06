using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Repositories
{
    public class DistributorRepository : IDistributorRepository
    {
        private readonly DistributorDbContext _ctx;
        private readonly IMapper _mapper;

        public DistributorRepository(DistributorDbContext ctx,IMapper mapper)
        {
            Preconditions.CheckNotNull(ctx,mapper);
            _mapper = mapper;
            _ctx = ctx;
        }

        public async Task<ScheduleDistributor.Domain.Models.Distributor> GetAsync(string id, CancellationToken cancellationToken)
        {
            var result = await  _ctx.Distributors.FirstOrDefaultAsync(o => o.Id.Equals(id), cancellationToken);
            return _mapper.Map<ScheduleDistributor.Domain.Models.Distributor>(result);
        }
        
        public async Task<ScheduleDistributor.Domain.Models.Distributor> CreateAsync(ScheduleDistributor.Domain.Models.Distributor distributor, CancellationToken cancellationToken)
        {
            var existedDistributor = await GetAsync(distributor.Id, cancellationToken);
            if(existedDistributor != null)
                throw new InvalidOperationException("Distributor with same id already exist!");

            _ctx.Distributors.Add(_mapper.Map<Entities.Distributor>(distributor));
            await _ctx.SaveChangesAsync(cancellationToken);
            return await GetAsync(distributor.Id, cancellationToken);
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            var existedPair = await _ctx.Distributors.FirstOrDefaultAsync(o => o.Id.Equals(id), cancellationToken);
            if (existedPair is null) throw new InvalidOperationException($"Can not delete PairInfo. PairInfo with id: {id} not exist");

            _ctx.Distributors.Remove(existedPair);
            await _ctx.SaveChangesAsync(cancellationToken);
        }
    }
}