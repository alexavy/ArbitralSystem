using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DistributorManagementService.Extensions.Serialisation
{
    internal static class DefaultJsonSerializerSettings
    {
        public static JsonSerializerSettings Create() => new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = false
                }
            },
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = new JsonConverter[]
            {
                new StringEnumConverter()
            }
        };

        public static void Apply(JsonSerializerSettings settings)
        {
            var defaultSettings = Create();

            settings.Formatting = defaultSettings.Formatting;
            settings.ContractResolver = defaultSettings.ContractResolver;
            settings.NullValueHandling = defaultSettings.NullValueHandling;
            settings.ReferenceLoopHandling = defaultSettings.ReferenceLoopHandling;
            settings.Converters = new JsonConverter[]
            {
                new StringEnumConverter()
            };
        }
    }
}