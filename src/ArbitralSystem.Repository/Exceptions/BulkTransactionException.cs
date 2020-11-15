using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ArbitralSystem.Domain.Common;

namespace ArbitralSystem.Repository.Exceptions
{
    public class BulkTransactionException<T> : Exception where T : class , IArbitralObject
    {
        private ConcurrentStack<T> _bag;

        public BulkTransactionException(string message) : base(message){}

        public BulkTransactionException(string message, Exception inner) : base(message, inner){}

        public BulkTransactionException(ConcurrentStack<T> bag, string message, Exception inner) : base(message, inner)
        {
            _bag = bag;
        }
        
        public BulkTransactionException(ConcurrentStack<T> bag, Exception inner) : base(string.Empty,inner)
        {
            _bag = bag;
        }
        
        public bool IsEntitiesInside => _bag != null && !_bag.IsEmpty;

        public IEnumerable<T> GetEntries()
        {
            return _bag;
        }
    }
}