using System;
using System.Collections;

namespace ArbitralSystem.Repository.Exceptions
{
    public class BulkRepositoryException : Exception
    {
        public BulkRepositoryException(string message) : base(message) { }

        public BulkRepositoryException(string message, Exception inner) : base(message, inner) { }
    }
}