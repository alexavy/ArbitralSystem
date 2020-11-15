using System;
using System.Linq.Expressions;
using ArbitralSystem.Distributor.MQDistributor.MQPersistence.Entities;
using JetBrains.Annotations;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.QueryHandlers.Specs
{
    public static class DistributorSpecs
    {
        public static Expression<Func<Entities.Distributor, bool>> ByType([CanBeNull] DistributorType? type)
        {
            if (!type.HasValue) return x => true;

            return distributor => distributor.Type == type.Value;
        }
        
        public static Expression<Func<Entities.Distributor, bool>> ByName([CanBeNull] string name)
        {
            if (string.IsNullOrEmpty(name)) return x => true;

            return distr => distr.Name.ToLower().StartsWith(name.ToLower());
        }
        
        public static Expression<Func<Entities.Distributor, bool>> ByStatus(Status? status)
        {
            if (!status.HasValue) return x => true;

            return distr => distr.Status == status.Value;
        }
        
        public static Expression<Func<Entities.Distributor, bool>> ByExceptStatus(Status? exceptStatus)
        {
            if (!exceptStatus.HasValue) return x => true;

            return distr => distr.Status != exceptStatus.Value;
        }
    }
}