using System;
using ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Persistence
{
    public class MarketInfoBdContext: DbContext
    {
        public MarketInfoBdContext(DbContextOptions<MarketInfoBdContext> options) : base(options)
        {
        }
        
        public DbSet<DistributorState> DistributerStates { get; set; }
        public DbSet<OrderbookPriceEntry> OrderbookPriceEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MarketInfoBdContext).Assembly, t => t.Namespace.Contains("Persistence"));
        }
    }
    
}