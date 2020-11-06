using System;
using ArbitralSystem.Domain.Distributers;
using ArbitralSystem.Domain.MarketInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Entities
{
    public class DistributerState : IEntityTypeConfiguration<DistributerState>
    {
        public Guid Id { get; set; }
        public string Symbol { get; set;}
        public Exchange Exchange { get; set;}
        public DateTimeOffset ChangedAt { get; set; }
        public DistributerSyncStatus PreviousStatus { get; set; }
        public DistributerSyncStatus CurrentStatus { get; set;}
        
        public void Configure(EntityTypeBuilder<DistributerState> builder)
        {
            builder.ToTable("DistributerStates")
                .HasKey(o => o.Id);
            
            builder.Property(o => o.Exchange)
                .HasConversion(x => x.ToString(),
                    x => (Exchange) Enum.Parse(typeof(Exchange), x))
                .IsUnicode(false);
        }
    }
}