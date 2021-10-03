using Erp.Data.Models.People;

namespace Erp.Data.Models.Users
{
    public class User : EntityBase<int>
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int PersonEmailAddressId { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public virtual Person Person { get; set; }
        public virtual PersonEmailAddress PersonEmailAddress { get; set; }
    }
}
