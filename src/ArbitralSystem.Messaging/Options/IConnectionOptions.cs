using System;

namespace ArbitralSystem.Messaging.Options
{
    public interface IConnectionOptions : ICloneable
    {
        string Host { get; }
        //sstring QuartzQueueName { get; }
    }
}