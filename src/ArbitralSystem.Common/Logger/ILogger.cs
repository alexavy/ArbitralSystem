using System;

namespace ArbitralSystem.Common.Logger
{
    public interface ILogger : IDisposable
    {
        object GetRootLogger();
        
        ILogger ForContext(string propertyName, object value);

        #region Verbose
        void Verbose(string messageTemplate);

        void Verbose(Exception exception, string messageTemplate);

        void Verbose(Exception exception, string messageTemplate, params object[] propertyValues);

        void Verbose(string messageTemplate, params object[] propertyValues);
        #endregion

        #region Debug
        void Debug(string messageTemplate);

        void Debug(Exception exception, string messageTemplate);

        void Debug(Exception exception, string messageTemplate, params object[] propertyValues);

        void Debug(string messageTemplate, params object[] propertyValues);
        #endregion

        #region Information
        void Information(string messageTemplate);

        void Information(Exception exception, string messageTemplate);

        void Information(Exception exception, string messageTemplate, params object[] propertyValues);

        void Information(string messageTemplate, params object[] propertyValues);
        #endregion

        #region Warning
        void Warning(string messageTemplate);

        void Warning(Exception exception, string messageTemplate);

        void Warning(Exception exception, string messageTemplate, params object[] propertyValues);

        void Warning(string messageTemplate, params object[] propertyValues);
        #endregion

        #region Error
        void Error(string messageTemplate);

        void Error(Exception exception, string messageTemplate);

        void Error(Exception exception, string messageTemplate, params object[] propertyValues);

        void Error(string messageTemplate, params object[] propertyValues);
        #endregion

        #region Fatal
        void Fatal(string messageTemplate);

        void Fatal(Exception exception, string messageTemplate);

        void Fatal(Exception exception, string messageTemplate, params object[] propertyValues);

        void Fatal(string messageTemplate, params object[] propertyValues);
        #endregion
    }
}
