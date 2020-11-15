using System;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence
{
    public class DistributorDbContext : DbContext
    {
        public DistributorDbContext(DbContextOptions<DistributorDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Exchange> Exchanges { get; set; }
        public DbSet<Entities.Distributor> Distributors { get; set; }
        public DbSet<Entities.DistributorExchange> DistributorExchanges { get; set; }
        public DbSet<Entities.Server> Servers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DistributorDbContext).Assembly, t => t.Namespace.Contains("Persistence"));
        }
    }
}