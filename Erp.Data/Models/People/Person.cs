using Erp.Data.Models.Users;
using System.Collections.Generic;

namespace Erp.Data.Models.People
{
    public class Person : EntityBase<int>
    {
        public Person()
        {
            Users = new HashSet<User>();
            PersonEmailAddresses = new HashSet<PersonEmailAddress>();
        }
        public int Id { get; set; }
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<PersonEmailAddress> PersonEmailAddresses { get; set; }
    }

    public enum Gender
    {
        Male = 1, Female = 2
    }
}
