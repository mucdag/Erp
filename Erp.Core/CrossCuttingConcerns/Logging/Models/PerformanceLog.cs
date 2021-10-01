namespace Core.CrossCuttingConcerns.Logging.Models
{
    public class PerformanceLog : InfoLog
    {
        public PerformanceLog(string className, string methodName, int totalSeconds, string? parameters = null, string? logInfo = null)
            : base(className, methodName, parameters, logInfo)
        {
            TotalSeconds = totalSeconds;
        }

        public int TotalSeconds { get; set; }

        //public PerformanceLog()
        //{
        //}
    }

    public class IotPerformanceLog : InfoLog
    {
        public IotPerformanceLog(string className, string methodName, int totalSeconds, string? parameters = null, string? logInfo = null)
            : base(className, methodName, parameters, logInfo)
        {
            TotalSeconds = totalSeconds;
        }

        public int TotalSeconds { get; set; }

        //public IotPerformanceLog()
        //{
        //}
    }

}