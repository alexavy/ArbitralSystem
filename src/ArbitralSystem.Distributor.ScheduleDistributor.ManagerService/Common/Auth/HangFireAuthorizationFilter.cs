using Hangfire.Dashboard;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Common.Auth
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize( DashboardContext context)
        {
            //can add some more logic here...
            return true;
        }
    }
}