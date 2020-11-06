using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence
{
    public class DistributorDbContext : DbContext
    {
        public DistributorDbContext(DbContextOptions<DistributorDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Distributor> Distributors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DistributorDbContext).Assembly, t => t.Namespace.Contains("Persistence"));
        }
    }
}