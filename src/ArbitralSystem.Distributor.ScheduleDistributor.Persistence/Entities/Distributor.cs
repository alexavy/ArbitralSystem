using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Entities
{
    [UsedImplicitly]
    public class Distributor : IEntityTypeConfiguration<Distributor>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DistributorType { get; set; }
        public string ServerName { get; set; }
        public string QueueName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        

        public void Configure(EntityTypeBuilder<Distributor> builder)
        {
            builder.ToTable("Distributors")
                .HasKey(o=>o.Id);
            
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.HasIndex(i => new {i.Name, i.DistributorType}).IsUnique();
        }
    }
}