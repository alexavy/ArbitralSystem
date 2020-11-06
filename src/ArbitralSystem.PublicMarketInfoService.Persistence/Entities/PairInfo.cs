using System;
using ArbitralSystem.Domain.MarketInfo;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Entities
{
    [UsedImplicitly]
    public class PairInfo : IEntityTypeConfiguration<PairInfo>
    {
        public Guid Id { get; set; }
        public string ExchangePairName { get; set; }
        
        public string UnificatedPairName { get; set; }
        public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set;}
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? DelistedAt { get; set; }
        public Exchange Exchange { get; set; }

        public void Configure(EntityTypeBuilder<PairInfo> builder)
        {
            builder.ToTable("PairInfos")
                .HasKey(o=>o.Id);
            
            builder.HasIndex(i => new {i.ExchangePairName, i.Exchange, i.DelistedAt}).IsUnique();

            builder.Property(o => o.Exchange)
                .HasConversion(x => x.ToString(),
                    x => (Exchange) Enum.Parse(typeof(Exchange), x))
                .IsUnicode(false);
        }
    }
}