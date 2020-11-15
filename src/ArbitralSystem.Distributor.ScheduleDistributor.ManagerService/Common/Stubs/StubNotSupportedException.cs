using System;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Common.Stubs
{    
    internal class StubNotSupportedException : Exception
    {
        private const string ErrorMessage = "Service stub, not supported in manager";
        public StubNotSupportedException() : base(ErrorMessage) { }
    }
}