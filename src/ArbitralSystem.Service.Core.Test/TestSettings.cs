using System;

namespace ArbitralSystem.Service.Core.Test
{
    internal class TestSettings : ICloneable
    {
        public string Connectivity { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}