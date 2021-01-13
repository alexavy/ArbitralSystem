using System;
using ArbitralSystem.Domain.MarketInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Entities
{
    public class OrderbookPriceEntry : IEntityTypeConfiguration<OrderbookPriceEntry>
    {
        public string Symbol { get; set; }
        public decimal Price { get;set;  }
        public decimal Quantity { get;set;  }
        public Exchange Exchange { get; set; }
        public Direction Direction { get; set; }
        public DateTime UtcCatchAt { get; set; }
        
        public void Configure(EntityTypeBuilder<OrderbookPriceEntry> builder)
        {
            builder.ToTable("OrderbookPriceEntries")
                .HasNoKey();
            
            builder.Property(o => o.Symbol)
                .HasColumnType("varchar(32)")
                .IsRequired();
            
            builder.Property(o => o.Quantity)
                .HasColumnType("decimal(19,9)")
                .IsRequired();
            
            builder.Property(o => o.Price)
                .HasColumnType("decimal(19,9)")
                .IsRequired();
            
            builder.Property(o => o.Direction)
                .HasColumnType("tinyint")
                .IsRequired();
            
            builder.Property(o => o.Exchange)
                .HasColumnType("tinyint")
                .IsRequired();
        }
    }
}