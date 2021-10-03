using Erp.Data.Models.Users;
using System.Collections.Generic;

namespace Erp.Data.Models.People
{
    public class PersonEmailAddress : EntityBase<int>
    {
        public PersonEmailAddress()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public int PersonId { get; set; }
        public string EmailAddress { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
