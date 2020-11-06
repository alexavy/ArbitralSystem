using System;
using System.Collections.Generic;
using ArbitralSystem.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitralSystem.Service.Core.Test
{
    [TestClass]
    public class SettingsLoggerTest
    {
        [TestMethod]
        public void HideRabbitMqPasswordTest()
        {
            var connectivity = new TestSettings()
            {
                Connectivity = "rabbitmq://login:password@localhost:5672"
            };
            
            SettingsLogger settingsLogger = new SettingsLogger(new EmptyLogger());

            var rule = new PasswordRegexRule()
            {
                RegexPattern = RegexPasswordPatterns.RabbitMq,
                NameOfProperty = "Connectivity",
                ObjectType = connectivity.GetType()
            };
            var rules = new Dictionary<Type, PasswordRegexRule>();
            rules.Add(rule.ObjectType, rule);
            var hidden = settingsLogger.HidePassword(rules, new ICloneable[] {connectivity});
            
            Assert.AreEqual( ((TestSettings)hidden[0]).Connectivity, "rabbitmq://********@localhost:5672" ); 
            Assert.AreEqual( connectivity.Connectivity, "rabbitmq://login:password@localhost:5672" );
        }
    }
}