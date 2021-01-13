using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.PublicMarketInfoService.Domain.Interfaces;
using ArbitralSystem.PublicMarketInfoService.Domain.Models;
using AutoMapper;
using EFCore.BulkExtensions;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Repositories
{
    public class PairPricesRepository : BaseRepository , IPairPricesRepository
    {
        private readonly ILogger _logger;
        public PairPricesRepository(PublicMarketInfoBdContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper)
        {
            Preconditions.CheckNotNull(logger);
            _logger = logger;
        }

        public async Task BulkSave(PairPrice[] pairPrices, CancellationToken token)
        {
            var prices = Mapper.Map<Entities.PairPrice[]>(pairPrices).ToArray();
            if (prices.Any())
            {
                await using (var transaction = await DbContext.Database.BeginTransactionAsync(token))
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    try
                    {
                        await DbContext.BulkInsertAsync(prices, cancellationToken: token);
                        await transaction.CommitAsync(token);
                    }
                    catch (Exception e)
                    {
                        watch.Stop();
                        await transaction.RollbackAsync(token);
                        _logger.Fatal(e, $"Error in Bulk insert pair prices, time for execution: {watch.ElapsedMilliseconds}");
                        throw;
                    }
                }
            }
        }
    }
}