using System;

namespace ArbitralSystem.Connectors.Core.Common
{
    public interface IResponse<T>
    {
        bool IsSuccess { get; }

        Exception? Exception { get; }

        T Data { get; }
    }
}