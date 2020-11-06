using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PublicMarketInfoBdContext>
    {
        public PublicMarketInfoBdContext CreateDbContext(string[] args)
        {
            return new PublicMarketInfoBdContext(new DbContextOptionsBuilder<PublicMarketInfoBdContext>()
                .UseSqlServer("Server=.\\SQLEXPRESS,1433;Database=PublicMarketInfoDb;User ID=sa;Password=12345678;MultipleActiveResultSets=true")
                .Options);
        }
    }
}