using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;

namespace ArbitralSystem.Common.Helpers
{
    public static class CertHelper
    {
        public static X509Certificate2 Get(IConfigurationRoot configuration)
        {
            var certificateSettings = configuration.GetSection("certificateSettings");
            var certificateFileName = certificateSettings.GetValue<string>("filename");
            var certificatePassword = certificateSettings.GetValue<string>("password");
            if(string.IsNullOrEmpty(certificateFileName) || string.IsNullOrEmpty(certificatePassword))
                throw  new AggregateException("Certificate file or pass not set.");
            return new X509Certificate2(certificateFileName, certificatePassword);
        }
    }
}