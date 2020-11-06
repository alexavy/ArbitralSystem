using System;

namespace ArbitralSystem.Connectors.Core.Models
{
    public interface IArbitralPairInfo : IPairInfo
    {
        DateTimeOffset CreatedAt { get; }

        DateTimeOffset? DelistedAt { get;  }
    }
}