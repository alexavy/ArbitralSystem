namespace ArbitralSystem.Common.Helpers
{
    public class ArbitralStopWatchResult<T> where T : class
    {
        public ArbitralStopWatchResult(T result, long elapsedTimeInMls)
        {
            Result = result;
            ElapsedInMls = elapsedTimeInMls;
        }
        
        public long ElapsedInMls { get; }
        public T Result { get; }
    }
}