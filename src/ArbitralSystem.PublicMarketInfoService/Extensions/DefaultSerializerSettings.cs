using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ArbitralSystem.PublicMarketInfoService.Extensions
{
    internal class DefaultSerializerSettings
    {
        public DefaultSerializerSettings(MvcNewtonsoftJsonOptions options)
        {
            options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}