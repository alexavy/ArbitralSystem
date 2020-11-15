using System;

namespace ArbitralSystem.Connectors.Core.Exceptions
{
    public class WebsocketException : Exception
    {
        public WebsocketException(string message) : base(message) { }

        public WebsocketException(string message, Exception inner) : base(message, inner) { }
    }
}
