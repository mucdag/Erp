using Newtonsoft.Json;

namespace Erp.Resource.Models.Users
{
    public class UserResource
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int PersonEmailAddressId { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
