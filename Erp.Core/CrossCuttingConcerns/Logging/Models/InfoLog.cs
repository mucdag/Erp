using System.Collections.Generic;

namespace Core.CrossCuttingConcerns.Logging.Models
{
    public class InfoLog : LogBase
    {
        public InfoLog(string className, string methodName, string? parameters = null, string? logInfo = null)
            : base(className, methodName, parameters, logInfo)
        {
        }

        //public InfoLog()
        //{
        //}
    }


    public class IotInfoLog : LogBase
    {
        public IotInfoLog(string className, string methodName, string? parameters = null, string? logInfo = null)
            : base(className, methodName, parameters, logInfo)
        {
        }

        //public IotInfoLog()
        //{
        //}
    }
}