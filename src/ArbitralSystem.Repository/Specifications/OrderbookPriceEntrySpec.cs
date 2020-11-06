using ArbitralSystem.Domain.Distributers.Models;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Repository.Specifications
{
    public static class OrderbookPriceEntrySpec
    {
        public static void SpecifyOrderbookPriceEntry(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderbookPriceEntry>()
                .ToTable("Stg_OrderbookPriceEntries")
                .HasKey(o => o.Oid);

            modelBuilder.Entity<OrderbookPriceEntry>()
                .Property(o => o.Oid)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<OrderbookPriceEntry>()
                .Property(o => o.Exchange)
                .HasColumnType("tinyint")
                .IsRequired();
            
            modelBuilder.Entity<OrderbookPriceEntry>()
                .Property(o => o.Symbol)
                .HasColumnType("varchar(16)")
                .HasMaxLength(12)
                .IsRequired();
            
            modelBuilder.Entity<OrderbookPriceEntry>()
                .Property(o => o.Quantity)
                .HasColumnType("decimal(19,9)")
                .IsRequired();
            
            modelBuilder.Entity<OrderbookPriceEntry>()
                .Property(o => o.Price)
                .HasColumnType("decimal(19,9)")
                .IsRequired();
            
            modelBuilder.Entity<OrderbookPriceEntry>()
                .Property(o => o.Direction)
                .HasColumnType("tinyint")
                .IsRequired();
            
            modelBuilder.Entity<OrderbookPriceEntry>()
                .Property(o => o.ReceivedTime)
                .HasColumnName("ReceivedDate")
                .HasColumnType("datetime2(4)")
                .IsRequired();
        }
    }
}