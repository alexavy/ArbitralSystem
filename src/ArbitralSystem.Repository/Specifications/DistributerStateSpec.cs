using ArbitralSystem.Domain.Distributers.Models;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Repository.Specifications
{
    public static class DistributerStateSpec
    {
        public static void SpecifyDistributerState(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DistributerState>().ToTable("Stg_DistributerStates")
                .HasKey(o => o.Oid);
        }
    }
}