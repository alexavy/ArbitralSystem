using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ArbitralSystem.Common.Helpers
{
    public static class ArbitralStopWatch
    {
        public static async Task<ArbitralStopWatchResult<T>> MeasureInMls<T>(Func<Task<T>> func) where T : class
        {
            var timer = new Stopwatch();
            timer.Start();
            var result = await func.Invoke();
            timer.Stop();
            return new ArbitralStopWatchResult<T>(result, timer.ElapsedMilliseconds);
        }
        
        public static async Task<long> MeasureInMls(Func<Task> func)
        {
            var timer = new Stopwatch();
            timer.Start();
            await func.Invoke();
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }
    }
}