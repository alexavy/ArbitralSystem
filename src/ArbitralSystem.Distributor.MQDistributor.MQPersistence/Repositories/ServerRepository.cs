using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Interfaces;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Repositories
{
    public class ServerRepository : IServerRepository
    {
        private readonly DistributorDbContext _ctx;
        private readonly IMapper _mapper;
        
        public ServerRepository(DistributorDbContext ctx,IMapper mapper)
        {
            Preconditions.CheckNotNull(ctx,mapper);
            _mapper = mapper;
            _ctx = ctx;
        }
        
        public async Task<Server> GetAsync(Guid id, CancellationToken token)
        {
            var result = await _ctx.Servers.FirstOrDefaultAsync(o => o.Id.Equals(id), token);
            return _mapper.Map<Server>(result);
        }

        public async Task<Server> CreateAsync(Server server, CancellationToken token)
        {
            var existedServer = await GetAsync(server.Id, token);
            if(existedServer != null)
                throw new InvalidOperationException("Server with same id already exist!");

            _ctx.Servers.Add(_mapper.Map<Entities.Server>(server));
            await _ctx.SaveChangesAsync(token);
            return await GetAsync(server.Id, token);
        }

        public async Task UpdateAsync(Server server, CancellationToken token)
        {
            var existedServer = await _ctx.Servers.FirstOrDefaultAsync(o => o.Id.Equals(server.Id), token);
            if(existedServer == null)
                throw new InvalidOperationException("Server with same id is not exist!");

            existedServer.Name = server.Name;
            existedServer.MaxWorkersCount = server.MaxWorkers;
            existedServer.CreatedAt = server.CreatedAt;
            existedServer.ModifyAt = server.ModifyAt;
            existedServer.IsDeleted = server.IsDeleted;
           
            await _ctx.SaveChangesAsync(token);
        }
    }
}