using System;
using ArbitralSystem.Domain.Distributers.Models;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Domain.MarketInfo.Models;
using ArbitralSystem.Repository.Specifications;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Repository
{
    [UsedImplicitly]
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        
        public DbSet<OrderbookPriceEntry> OrderbookPriceEntries { get; set; }
        public DbSet<DistributerState> DistributerStates { get; set; }
        //public DbSet<PairInfo> PairInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SpecifyOrderbookPriceEntry();
            modelBuilder.SpecifyDistributerState();
            modelBuilder.SpecifyPairInfo();
        }
    }
}