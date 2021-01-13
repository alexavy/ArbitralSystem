using System;
using System.Linq.Expressions;
using ArbitralSystem.Distributor.MQDistributor.MQPersistence.Entities;
using JetBrains.Annotations;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.QueryHandlers.Specs
{
    public class ServerSpecs
    {
        public static Expression<Func<Entities.Server, bool>> ByType([CanBeNull] ServerType? serverType)
        {
            if (!serverType.HasValue) return x => true;

            return server => server.ServerType == serverType.Value;
        }
        
        public static Expression<Func<Entities.Server, bool>> ByIsDeleted([CanBeNull] bool? isDeleted)
        {
            if (!isDeleted.HasValue) return x => true;

            return server => server.IsDeleted == isDeleted.Value;
        }
    }
}