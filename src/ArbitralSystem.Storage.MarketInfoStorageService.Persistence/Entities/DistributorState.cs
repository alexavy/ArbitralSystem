using System;
using ArbitralSystem.Domain.Distributers;
using ArbitralSystem.Domain.MarketInfo;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Entities
{
    [UsedImplicitly]
    public class DistributorState : IEntityTypeConfiguration<DistributorState>
    {
        public Int32 Id { get; set; }
        public string Symbol { get; set;}
        public Exchange Exchange { get; set;}
        public DateTime UtcChangedAt { get; set; }
        public DistributerSyncStatus PreviousStatus { get; set; }
        public DistributerSyncStatus CurrentStatus { get; set;}
        
        public void Configure(EntityTypeBuilder<DistributorState> builder)
        {
            builder.ToTable("DistributerStates")
                .HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .UseIdentityColumn();
            
            builder.Property(o => o.Symbol)
                .HasColumnType("varchar(32)")
                .IsRequired();
            
            builder.Property(o => o.Exchange)
                .HasColumnType("tinyint")
                .IsRequired();
            
            builder.Property(o => o.PreviousStatus)
                .HasColumnType("tinyint")
                .IsRequired();
            
            builder.Property(o => o.CurrentStatus)
                .HasColumnType("tinyint")
                .IsRequired();
        }
    }
}