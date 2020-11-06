using System;
using System.Linq.Expressions;
using Hangfire.Annotations;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.QueryHandlers.Specs
{
    public static class DistributorSpecs
    {
        public static Expression<Func<Entities.Distributor, bool>> ByType([CanBeNull] string? type)
        {
            if (string.IsNullOrEmpty(type)) return x => true;

            return distr => distr.DistributorType.ToLower().Equals(type.ToLower());
        }
        
        public static Expression<Func<Entities.Distributor, bool>> ByServerName([CanBeNull] string? serverName)
        {
            if (string.IsNullOrEmpty(serverName)) return x => true;

            return distr => distr.ServerName.ToLower().StartsWith(serverName.ToLower());
        }
        
        public static Expression<Func<Entities.Distributor, bool>> ByQueueName([CanBeNull] string? queueName)
        {
            if (string.IsNullOrEmpty(queueName)) return x => true;

            return distr => distr.QueueName.ToLower().StartsWith(queueName.ToLower());
        }
        
        public static Expression<Func<Entities.Distributor, bool>> ByName([CanBeNull] string? name)
        {
            if (string.IsNullOrEmpty(name)) return x => true;

            return distr => distr.Name.ToLower().Contains(name.ToLower());
        }
    }
}