using System;
using ArbitralSystem.Domain.MarketInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Entities
{
    public class PairPrice : IEntityTypeConfiguration<PairPrice>
    {
        public string ExchangePairName { get; set; }
        public decimal? Price { get; set; }
        public Exchange Exchange { get; set; }
        public DateTimeOffset Date { get; set; }
        
        public void Configure(EntityTypeBuilder<PairPrice> builder)
        {
            builder.ToTable("PairPrices")
                .HasNoKey();
            
            builder.Property(o => o.ExchangePairName)
                .HasColumnType("varchar(16)")
                .HasMaxLength(12)
                .IsRequired();

            builder.Property(o => o.Price)
                .HasColumnType("decimal(19,9)");
        }
    }
}