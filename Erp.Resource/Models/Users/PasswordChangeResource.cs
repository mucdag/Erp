using Newtonsoft.Json;

namespace Erp.Resource.Models.Users
{
    public class PasswordChangeResource
    {
        [JsonIgnore]
        public string OldPassword { get; set; }
        [JsonIgnore]
        public string NewPassword { get; set; }
        [JsonIgnore]
        public string ConfirmNewPassword { get; set; }
    }
}
