using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArbitralSystem.Common.Helpers
{
    public static class ArbitralPolly
    {
        private static readonly ArbitralPollySettings DefaultPollySettings;
        static ArbitralPolly()
        {
            DefaultPollySettings = new ArbitralPollySettings(10, TimeSpan.FromMinutes(10), TimeSpan.FromSeconds(5));
        }
        
        public static async Task InfiniteRetry<T>(Task task, CancellationToken token, Action<int,TimeSpan> intermediateLog, ArbitralPollySettings settings = null)
            where T : Exception
        {
            if (settings is null)
                settings = DefaultPollySettings;
            await Retry<T>(task, token, intermediateLog, settings);
        }

        private static async Task Retry<T>(Task task, CancellationToken token, Action<int,TimeSpan> intermediateLog, ArbitralPollySettings settings) where T : Exception
        {
            int i = 0;
            var datetime = DateTimeOffset.Now;
            var ex = new Exception();
            while (i <= settings.RetryNumber && !token.IsCancellationRequested)
            {
                try
                {
                    await task;
                }
                catch (T e)
                {
                    ex = e;
                    intermediateLog.Invoke(i, datetime - DateTimeOffset.Now);
                }

                /*if (datetime - DateTimeOffset.Now > settings.LifeTime)
                {
                    datetime = DateTimeOffset.Now;
                    i = 0;
                }*/

                if (!token.IsCancellationRequested)
                    await Task.Delay(settings.Delay, token);
            }

            if (!token.IsCancellationRequested)
                throw new Exception($"Task attained max retry limit : [{i}/10] in 10 minutes", ex);
        }
    }
}