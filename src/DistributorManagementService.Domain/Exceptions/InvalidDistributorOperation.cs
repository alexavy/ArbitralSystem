using System;

namespace DistributorManagementService.Domain.Exceptions
{
    public class InvalidDistributorOperation :Exception
    {
        public InvalidDistributorOperation(string message) : base(message) { }

        public InvalidDistributorOperation(string message, Exception inner) : base(message, inner) { }
    }
}