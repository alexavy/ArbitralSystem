using System;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using JetBrains.Annotations;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Mapping.AuxiliaryModels
{
    [UsedImplicitly]
    public class DistributorAuxiliaryModel : IDistributor
    {
        public string Id { get; set; }
        public string Name { get; set;}
        public string DistributorType { get; set;}
        public string ServerName { get; set;}
        public string QueueName { get;set; }
        public DateTimeOffset CreatedAt { get; set;}
    }
}