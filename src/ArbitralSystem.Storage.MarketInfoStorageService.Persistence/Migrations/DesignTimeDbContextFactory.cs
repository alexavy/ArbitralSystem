using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MarketInfoBdContext>
    {
        public MarketInfoBdContext CreateDbContext(string[] args)
        {
            return new MarketInfoBdContext(new DbContextOptionsBuilder<MarketInfoBdContext>()
                .UseSqlServer("Server=.\\SQLEXPRESS,1433;Database=MarketInfoStorageDb;User ID=sa;Password=12345678;MultipleActiveResultSets=true")
                .Options);
        }
    }
}