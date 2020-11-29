using System;
using ArbitralSystem.Domain.MarketInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Entities
{
    public class PairPrice : IEntityTypeConfiguration<PairPrice>
    {
        public string ExchangePairName { get; set; } // TODO: rename to ExchangeSymbol
        public decimal? Price { get; set; }
        public Exchange Exchange { get; set; }
        public DateTime UtcDate { get; set; }
        
        public void Configure(EntityTypeBuilder<PairPrice> builder)
        {
            builder.ToTable("PairPrices")
                .HasNoKey();
            
            builder.Property(o => o.ExchangePairName)
                .HasColumnType("varchar(32)") // TODO: update max length
                .HasMaxLength(16)
                .IsRequired();
            
            builder.Property(o => o.Price)
                .HasColumnType("decimal(19,9)");
            
            builder.Property(o => o.Exchange)
                .HasColumnType("tinyint")
                .IsRequired();
            
            builder.Property(o => o.UtcDate)
                .IsRequired().HasColumnType("smalldatetime");
        }
    }
}
