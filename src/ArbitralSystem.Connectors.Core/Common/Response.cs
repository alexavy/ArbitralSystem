using System;

namespace ArbitralSystem.Connectors.Core.Common
{


    public class Response<T> : IResponse<T>
    {
        private readonly bool _isSuccess;
        private readonly Exception? _exception;
        private readonly string _exceptionMessage;
        private readonly T _data;

        public Response(Exception exception)
        {
            _isSuccess = false;
            _exception = exception;
            _exceptionMessage = exception.Message;
        }

        public Response(T data)
        {
            _isSuccess = true;
            _exception = null;
            _exceptionMessage = null;
            _data = data;
        }

        public Response()
        {
            _isSuccess = true;
            _exception = null;
            _exceptionMessage = null;
        }

        public bool IsSuccess { get => _isSuccess; }

        public Exception? Exception { get => _exception; }

        public string ExceptionMessage { get => _exceptionMessage; }

        public T Data {get => _data; }
    }
}
