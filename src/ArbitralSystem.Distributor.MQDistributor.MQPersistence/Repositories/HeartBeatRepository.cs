using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Interfaces;
using ArbitralSystem.Distributor.MQDistributor.MQPersistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Repositories
{
    public class HeartBeatRepository : IHeartBeatRepository
    {
        private readonly DistributorDbContext _ctx;
        private readonly ILogger _logger;

        public HeartBeatRepository(DistributorDbContext ctx, ILogger logger)
        {
            Preconditions.CheckNotNull(ctx, logger);
            _ctx = ctx;
            _logger = logger;
        }
        
        public async Task Update(DistributorHeartBeat[] heartBeats, CancellationToken token)
        {
            await using var transaction = await _ctx.Database.BeginTransactionAsync(token).ConfigureAwait(false);
            try
            {
                foreach (var heartBeat in heartBeats)
                {
                    var distributorExchange = await _ctx.DistributorExchanges
                        .FirstOrDefaultAsync(o => o.DistributorId == heartBeat.DistributorId && o.Exchange.Name == heartBeat.Exchange.ToString(),token)
                        .ConfigureAwait(false);
;                    if (distributorExchange != null)
                    {
                        distributorExchange.HeartBeat = heartBeat.DateTimeOffset;
                    }
                }
                await _ctx.SaveChangesAsync(token).ConfigureAwait(false);
                await transaction.CommitAsync(token).ConfigureAwait(false);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(token).ConfigureAwait(false);
                throw;
            }
        }
    }
}