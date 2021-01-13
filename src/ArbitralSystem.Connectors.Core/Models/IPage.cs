using System.Collections.Generic;

namespace ArbitralSystem.Connectors.Core.Models
{
    public interface IPage<out T>
    {
        int Total { get; }

        IEnumerable<T> Items { get; }
    }
}