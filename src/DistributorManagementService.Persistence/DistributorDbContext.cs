using DistributorManagementService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace DistributorManagementService.Persistence
{
    
    public class DistributorDbContext: DbContext
    {
        public DistributorDbContext(DbContextOptions<DistributorDbContext> options) : base(options)
        {
        }
        
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<OrderBookDistributor> OrderBookDistributors { get; set; }
        public DbSet<OrderBookDistributorProperty> OrderBookDistributorProperties { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DistributorDbContext).Assembly, t => t.Namespace.Contains("Persistence"));
        }
    }
    
}