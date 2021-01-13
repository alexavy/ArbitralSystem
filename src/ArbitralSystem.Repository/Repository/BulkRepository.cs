using System;
using ArbitralSystem.Common.Logger;
using EFCore.BulkExtensions;
using System.Collections.Concurrent;
using ArbitralSystem.Domain.Common;
using ArbitralSystem.Repository.Exceptions;

namespace ArbitralSystem.Repository.BulkRepository
{
    public class BulkRepository<T> : IBulkRepository<T> where T : class , IArbitralObject
    {
        private readonly ILogger _logger;
        private DataBaseContext _ctx;
        
        public BulkRepository(DataBaseContext ctx,
            ILogger logger)
        {
            _logger = logger;
            _ctx = ctx;
        }
        
        public void Save(T[] objs)
        {
            
            try
            {
                using (var transaction = _ctx.Database.BeginTransaction())
                {
                    _ctx.BulkInsert(objs);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                watch.Stop();//ONLY FOR DEBUG, remove later
                var elapsedErrorMs = watch.ElapsedMilliseconds;//ONLY FOR DEBUG, remove later
                string errorMessage = "$Error in Bulk insert, time for execution: {elapsedErrorMs}";
                _logger.Fatal(e, errorMessage);
                throw new BulkTransactionException<T>(new ConcurrentStack<T>(objs), errorMessage, e);
            }
            watch.Stop();//ONLY FOR DEBUG, remove later
            var elapsedMs = watch.ElapsedMilliseconds;//ONLY FOR DEBUG, remove later
            _logger.Information($"Orderbook entries saved, time in mls: {elapsedMs}");//remove
        }
        
    }
}
