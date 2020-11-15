using System;

namespace ArbitralSystem.Distributer.PairInfoDistributerService.Options
{
    public interface IPairInfoDistributerOptions : ICloneable
    {
        SiftType SiftType { get; }
    }
}