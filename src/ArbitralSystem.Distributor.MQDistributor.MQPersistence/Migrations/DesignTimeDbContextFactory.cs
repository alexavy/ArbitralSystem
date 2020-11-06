using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DistributorDbContext>
    {
        public DistributorDbContext CreateDbContext(string[] args)
        {
            return new DistributorDbContext(new DbContextOptionsBuilder<DistributorDbContext>()
                .UseSqlServer("Server=.\\SQLEXPRESS,1433;Database=DistributorDb;User ID=sa;Password=12345678;MultipleActiveResultSets=true")
                .Options);
        }
    }
}