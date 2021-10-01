namespace Core.CrossCuttingConcerns.Logging.Models
{
    public class LogQueryOption
    {
        public int Skip { get; set; }
        public int Take { get; set; } = 10;
        public string User { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public LogQueryOptionOrder LogQueryOptionOrder { get; set; }
    }

    public enum LogQueryOptionOrder
    {

    }
}
