using System;

namespace ArbitralSystem.Distributor.MarketInfoAggregatorService.Services
{
    public delegate void AggregateCollectionFilledDelegate<in T>(T[] objs) where T : class;
    public interface IAggregator<T> : IDisposable where T : class
    {
        void Add(T obj);
        void Add(T[] objs);
        T[] Take();

        event AggregateCollectionFilledDelegate<T> Filled;
    }
}