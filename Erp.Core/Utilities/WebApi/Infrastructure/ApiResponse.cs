using System.Net;
using System.Runtime.Serialization;

namespace Core.Utilities.WebApi.Infrastructure
{
    [DataContract]
    public class ApiResponse
    {
        public ApiResponse(HttpStatusCode statusCode, object? result = null, string? errorMessage = null)
        {
            StatusCode = (int) statusCode;
            ErrorMessage = errorMessage;
            Result = result;
        }

        [DataMember] public string Version => "1.0";

        [DataMember] public int StatusCode { get; set; }

        [DataMember(EmitDefaultValue = false)] public string ErrorMessage { get; set; }

        [DataMember(EmitDefaultValue = false)] public object Result { get; set; }
    }
}