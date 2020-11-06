using System;

namespace ArbitralSystem.Connectors.Core.Exceptions
{
    public class UnexpectedRestClientException : RestClientException
    {
        public UnexpectedRestClientException(string message) : base(message) { }

        public UnexpectedRestClientException(string message, Exception inner) : base(message, inner) { }
    }
}
