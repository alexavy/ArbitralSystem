using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ArbitralSystem.Common.Logger;

namespace ArbitralSystem.Service.Core
{
    public class SettingsLogger
    {
        private readonly ILogger _logger;
        private const string HiddenPassword = "********";
        
        public SettingsLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void PasswordHiddenInfo(Dictionary<Type, PasswordRegexRule> regexPasswordRules, params ICloneable[] objs )
        {
           var hiddenObj = HidePassword(regexPasswordRules, objs);
           Info(hiddenObj);
        }

        public void Info(params object[] objs)
        {
            var logString = new StringBuilder();
            int counter = 0;
            foreach (var obj in objs)
            {
                logString.Append($"{(obj.GetType())} : {{@t{++counter}}}\n");
            }
            _logger.Information(logString.ToString(),objs);
        }

        public object[] HidePassword(Dictionary<Type, PasswordRegexRule> regexPasswordRules ,ICloneable[] objs)
        {
            List<object> hiddenObjs = new List<object>();
            foreach (var @object in objs)
            {
                var obj = @object.Clone();
                var objectType = obj.GetType();
                if (regexPasswordRules.ContainsKey(objectType))
                {
                    var rule = regexPasswordRules[objectType];
                    var objectWithPassword = objectType.GetProperty(rule.NameOfProperty)?.GetValue(obj,null);
                    
                    if(objectWithPassword == null)
                        continue;
                    
                    var objectWithHiddenPassword = Regex.Replace(objectWithPassword.ToString(), rule.RegexPattern, HiddenPassword);
                    PropertyInfo prop = objectType.GetProperty(rule.NameOfProperty);
                    prop?.SetValue (obj, objectWithHiddenPassword, null);
                }
                hiddenObjs.Add(obj);
            }
            return hiddenObjs.ToArray();
        }

        public string HidePassword(string connectionString, string pattern)
        {
            return Regex.Replace(connectionString, pattern, HiddenPassword);
        }
    }
}