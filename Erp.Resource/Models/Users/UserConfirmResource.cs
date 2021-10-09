using Newtonsoft.Json;

namespace Erp.Resource.Models.Users
{
    public class UserConfirmResource
    {
        public string EmailAddress { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public string ConfirmPassword { get; set; }

        [JsonIgnore]
        public bool ValidatePassword { get; set; }
    }
}
