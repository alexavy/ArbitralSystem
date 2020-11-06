
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;



namespace ArbitralSystem.Common.Logger
{
    public class LoggerFactory
    {
        private readonly IConfiguration _configuration;

        public LoggerFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ArbitralSystem.Common.Logger.ILogger GetInstance()
        {
            return new LoggerWrapper( new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger());
        }
    }
}
