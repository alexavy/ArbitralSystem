using ArbitralSystem.PublicMarketInfoService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.PublicMarketInfoService.Persistence
{
    public class PublicMarketInfoBdContext : DbContext
         {
             public PublicMarketInfoBdContext(DbContextOptions<PublicMarketInfoBdContext> options) : base(options)
             {
             }
     
             public DbSet<PairInfo> PairInfos { get; set; }
             public DbSet<PairPrice> PairPrices { get; set; }
             protected override void OnModelCreating(ModelBuilder modelBuilder)
             {
                 modelBuilder.ApplyConfigurationsFromAssembly(typeof(PublicMarketInfoBdContext).Assembly, t => t.Namespace.Contains("Persistence"));
             }
         }
}