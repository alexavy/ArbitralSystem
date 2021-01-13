using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Entities
{
    [UsedImplicitly]
    public class DistributorExchange : IEntityTypeConfiguration<DistributorExchange>
    {
        public Guid DistributorId { get; set; }
        public virtual Distributor Distributor { get; set; }
        public int ExchangeId { get; set; }
        public Exchange Exchange { get; set; }
        public DateTimeOffset? HeartBeat { get; set; }
        public void Configure(EntityTypeBuilder<DistributorExchange> builder)
        {
            builder.ToTable("DistributorExchanges", "mqd");
            builder.HasKey(x => new {x.DistributorId, x.ExchangeId});
        }
    }
}