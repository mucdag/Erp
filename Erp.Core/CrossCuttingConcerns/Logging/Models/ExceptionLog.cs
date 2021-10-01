using System;

namespace Core.CrossCuttingConcerns.Logging.Models
{
    public class ExceptionLog : InfoLog
    {
        public ExceptionLog(string className, string methodName,string parameters,
            Exception exception, string? logInfo = null)
            : base(className, methodName, parameters, logInfo)
        {
            Exception = exception.ToString();
        }

        public string Exception { get; set; }
    }

    public class IotExceptionLog : InfoLog
    {
        public IotExceptionLog(string className, string methodName, string parameters,
            Exception exception, string? logInfo = null)
            : base(className, methodName, parameters, logInfo)
        {
            Exception = exception.ToString();
        }

        public string Exception { get; set; }
    }
}