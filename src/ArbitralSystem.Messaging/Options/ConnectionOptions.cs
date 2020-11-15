using JetBrains.Annotations;

namespace ArbitralSystem.Messaging.Options
{
    [UsedImplicitly]
    public class ConnectionOptions : IConnectionOptions
    {
        public string Host { get; set; }
        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}