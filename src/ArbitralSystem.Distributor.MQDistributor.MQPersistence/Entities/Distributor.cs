using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Entities
{
    [UsedImplicitly]
    public class Distributor : IEntityTypeConfiguration<Distributor>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DistributorType Type { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifyAt { get; set; }
        public Status Status { get; set; }
        public Guid? ServerId { get; set; }
        public Server Server { get; set; }
        public virtual ICollection<DistributorExchange> Exchanges { get; set; }
        
        public void Configure(EntityTypeBuilder<Distributor> builder)
        {
            builder.ToTable("Distributors","mqd")
                .HasKey(o=>o.Id);
            
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.Property(o => o.Name).IsRequired();
            builder.Property(o => o.Type).IsRequired();

            builder.Property(o => o.Type)
                .HasConversion(x => x.ToString(),
                    x => (DistributorType) Enum.Parse(typeof(DistributorType), x))
                .IsUnicode(false);
            
            builder.Property(o => o.Status)
                .HasConversion(x => x.ToString(),
                    x => (Status) Enum.Parse(typeof(Status), x))
                .IsUnicode(false);
            
            builder.HasOne(e => e.Server)
                .WithMany(c => c.Distributors);
        }
    }
}