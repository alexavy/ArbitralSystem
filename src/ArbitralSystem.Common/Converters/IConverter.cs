using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitralSystem.Common.Converters
{
    public interface IConverter
    {
        TDestination Convert<TSource, TDestination>(TSource source);
    }
}
