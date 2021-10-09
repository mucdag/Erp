using Newtonsoft.Json;

namespace Erp.Resource.Models.Users
{
    public class PasswordResetResource
    {
        public string EmailAddress { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string ResetCode { get; set; }
    }
}
