using System.Collections.Concurrent;
using System.Linq;
using System.Timers;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Distributor.MarketInfoAggregatorService.Common;

namespace ArbitralSystem.Distributor.MarketInfoAggregatorService.Services
{
    public class TimeLimitedAggregatorStack<T> : ITimeLimitedAggregator<T> where T : class
    {
        private readonly AggregatorOptions _options;
        private readonly ILogger _logger;

        private ConcurrentStack<T> _bag;
        private Timer _timer;

        public TimeLimitedAggregatorStack(AggregatorOptions options,
            ILogger logger)
        {
            _options = options;
            _logger = logger;
            _bag = new ConcurrentStack<T>();
        }

        private object _lockObj = new object();

        public void StartTimer()
        {
            _timer = new Timer(_options.TimeBaseCleansing.TotalMilliseconds);
            _timer.Elapsed += _timer_Elapsed;
        }

        public void StopTimer()
        {
            _timer?.Stop();
        }


        public void Add(T obj)
        {
            AddElementOrRiseEvent(new [] {obj});
        }

        public void Add(T[] objs)
        {
            AddElementOrRiseEvent(objs);
        }

        public T[] Take()
        {
            lock (_lockObj)
                return _bag.ToArray();
        }

        private void AddElementOrRiseEvent(T[] objs)
        {
            if (objs.Any())
            {
                lock (_lockObj)
                {
                    _bag.PushRange(objs);
                    _logger.Verbose($"Message package: {objs.Count()} added to bag: {_bag.Count()}:{_options.Limit}");
                    if (_bag.Count >= _options.Limit)
                    {
                        RaiseEventAndClear();
                    }
                }
            }
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (_lockObj)
            {
                RaiseEventAndClear();
            }
        }

        private void RaiseEventAndClear()
        {
            if (_bag.Count != 0)
            {
                OnFilled(new ConcurrentStack<T>(_bag).ToArray());
                _bag.Clear();
            }

            ResetTimer();
        }

        private void ResetTimer()
        {
            _timer?.Stop();
            _timer?.Start();
        }

        private event AggregateCollectionFilledDelegate<T> AggregateCollectionFilledHandler;

        public event AggregateCollectionFilledDelegate<T> Filled
        {
            add => AggregateCollectionFilledHandler += value;
            remove => AggregateCollectionFilledHandler -= value;
        }

        private void OnFilled(T[] objs)
        {
            AggregateCollectionFilledHandler?.Invoke(objs);
        }

        public void Dispose()
        {
            lock (_lockObj)
                _timer?.Stop();

            _bag.Clear();
        }
    }
}