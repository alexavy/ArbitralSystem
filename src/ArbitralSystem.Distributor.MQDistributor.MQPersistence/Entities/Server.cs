using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Entities
{
    [UsedImplicitly]
    public class Server: IEntityTypeConfiguration<Server>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ServerType ServerType { get; set; }
        public int MaxWorkersCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifyAt { get; set; }
        public ICollection<Distributor> Distributors { get; set; }
        public bool IsDeleted { get; set; }
        
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.ToTable("Servers","mqd")
                .HasKey(o=>o.Id);
            
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.Property(o => o.Name).IsRequired();

            builder.HasMany(x => x.Distributors)
                .WithOne(b => b.Server)
                .HasForeignKey(b => b.ServerId);
            
            builder.Property(o => o.ServerType)
                .HasConversion(x => x.ToString(),
                    x => (ServerType) Enum.Parse(typeof(ServerType), x))
                .IsUnicode(false);
        }
    }
}