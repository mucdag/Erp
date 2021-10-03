using System;
using Core.Utilities;

namespace Core.CrossCuttingConcerns.Logging.Models
{
	public class LogBase
	{
		public LogBase(string className, string methodName, string parameters, string? logInfo = null)
		{
			LogInfo = logInfo;
			Date = DateTime.Now;
			ClassName = className;
			MethodName = methodName;
			Parameters = parameters;
		}
		
		public string ClassName { get; set; }
		public string MethodName { get; set; }
		public string Parameters { get; set; }
		public string LogInfo { get; set; }
		public string User { get; set; } = CurrentUser.UserIdentity?.User;
		public string UserAgent { get; set; } = CurrentUser.UserAgent;
		public string Language { get; set; } = CurrentUser.Language;
		public string IPAddresses { get; set; } = CurrentUser.IPAdresses;
		public string RequestId { get; set; } = CurrentUser.RequestId;
		public DateTime Date { get; set; }

	}
}