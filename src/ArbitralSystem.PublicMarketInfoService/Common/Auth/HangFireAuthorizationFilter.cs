using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;

namespace ArbitralSystem.PublicMarketInfoService.Common.Auth
{

    internal class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize( DashboardContext context)
        {
            //can add some more logic here...
            return true;
        }
    }
    
}