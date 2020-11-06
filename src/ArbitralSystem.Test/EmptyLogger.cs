using System;
using ArbitralSystem.Common.Logger;

namespace ArbitralSystem.Test
{
    public class EmptyLogger : ILogger
    {
        public void Debug(string messageTemplate)
        {
            
        }

        public void Debug(Exception exception, string messageTemplate)
        {
            
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Error(string messageTemplate)
        {
            
        }

        public void Error(Exception exception, string messageTemplate)
        {
            
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Fatal(string messageTemplate)
        {
            
        }

        public void Fatal(Exception exception, string messageTemplate)
        {
            
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            
        }

        public object GetRootLogger()
        {
            return null;
        }

        public ILogger ForContext(string propertyName, object value)
        {
            return this;
        }

        public void Information(string messageTemplate)
        {
            
        }

        public void Information(Exception exception, string messageTemplate)
        {
            
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Verbose(string messageTemplate)
        {
            
        }

        public void Verbose(Exception exception, string messageTemplate)
        {
            
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Warning(string messageTemplate)
        {
            
        }

        public void Warning(Exception exception, string messageTemplate)
        {
            
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
