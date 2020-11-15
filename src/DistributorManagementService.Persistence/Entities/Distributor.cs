using System;
using System.Collections.Generic;
using ArbitralSystem.Domain.Distributers;
using ArbitralSystem.Domain.MarketInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DistributorManagementService.Persistence.Entities
{
    public class Distributor : IEntityTypeConfiguration<Distributor>
    {
        public Guid Id { get; set; }
        public DistributorState DistributorState { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? OrderBookDistributorId { get; set; }
        public OrderBookDistributor OrderBookDistributor { get; set; }
        
        public void Configure(EntityTypeBuilder<Distributor> builder)
        {
            builder.ToTable("Distributors")
                .HasKey(o => o.Id);

            builder.HasIndex(o => o.DistributorState);

            builder.Property(o => o.DistributorState)
                .HasConversion(x => x.ToString(),
                    x => (DistributorState) Enum.Parse(typeof(Exchange), x))
                .IsUnicode(false);
            
            builder.HasOne(o => o.OrderBookDistributor)
                .WithOne(o => o.Distributor)
                .HasForeignKey<OrderBookDistributor>(k => k.DistributorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class OrderBookDistributor: IEntityTypeConfiguration<OrderBookDistributor> 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnificatedPair { get; set; }
        public Guid DistributorId { get; set; }
        public Distributor  Distributor { get; set; }
        public ICollection<OrderBookDistributorProperty> OrderBookDistributorProperties { get; set; }
        
        public void Configure(EntityTypeBuilder<OrderBookDistributor> builder)
        {
            builder.ToTable("OrderBookDistributors")
                .HasKey(o => o.Id);
            
            builder.HasIndex(o => o.UnificatedPair);
            
            builder.HasMany(o => o.OrderBookDistributorProperties)
                .WithOne(o => o.OrderBookDistributor)
                .HasForeignKey(k => k.OrderBookDistributorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class OrderBookDistributorProperty : IEntityTypeConfiguration<OrderBookDistributorProperty> 
    {
        public Guid Id { get; set; }
        public string ExchangePairName { get; set; }
        public Exchange Exchange { get; set; }
        public Guid OrderBookDistributorId { get; set; }
        public OrderBookDistributor OrderBookDistributor { get; set; }
        
        public void Configure(EntityTypeBuilder<OrderBookDistributorProperty> builder)
        {
            builder.ToTable("OrderBookProperties").HasKey(o => o.Id);
            
            builder.Property(o => o.Exchange)
                .HasConversion(x => x.ToString(),
                    x => (Exchange) Enum.Parse(typeof(Exchange), x))
                .IsUnicode(false);
        }
    }
}