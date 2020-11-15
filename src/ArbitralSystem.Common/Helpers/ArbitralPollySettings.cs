using System;

namespace ArbitralSystem.Common.Helpers
{
    public class ArbitralPollySettings
    {
        public ArbitralPollySettings(uint retryNumber, TimeSpan lifeTime, TimeSpan delay)
        {
            if(retryNumber == 0 )
                throw new ArgumentException("Retry number must be positive value.");

            if (lifeTime <= delay)
                throw new ArgumentException("Life time should be greater than delay");

            RetryNumber = retryNumber;
            LifeTime = lifeTime;
            Delay = delay;
        }
        public uint RetryNumber { get; }
        public TimeSpan LifeTime { get; }
        public TimeSpan Delay { get; }
    }
}