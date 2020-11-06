using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo.Models;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Repository.Specifications
{
    public static class PairInfoSpec
    {
        public static void SpecifyPairInfo(this ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<PairInfo>()
                .ToTable("Stg_PairInfos")
                .HasKey(o=>o.Oid);*/

            modelBuilder.Entity<PairInfo>()
                .Ignore(o => o.UnificatedPairName);
        }
    }
}