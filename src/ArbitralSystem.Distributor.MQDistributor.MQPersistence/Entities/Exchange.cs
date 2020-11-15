using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Entities
{
    [UsedImplicitly]
    public class Exchange : IEntityTypeConfiguration<Exchange>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DistributorExchange> Distributors { get; set; }

        public void Configure(EntityTypeBuilder<Exchange> builder)
        {
            builder.ToTable("Exchanges", "mqd")
                .HasKey(o => o.Id);

            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.Property(o => o.Name).IsRequired();
            builder.HasIndex(o => o.Name).IsUnique();
        }
    }
}