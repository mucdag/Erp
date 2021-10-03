using Erp.Data.Models.People;
using Microsoft.EntityFrameworkCore;
using Erp.Data.Repositories.WriteRepositories;

namespace Erp.Data.Repositories.WriteRepository.People
{
    public class PersonEmailAddressWriteRepository : BaseWriteRepository<PersonEmailAddress, int>, IPersonEmailAddressWriteRepository
    {
        public PersonEmailAddressWriteRepository(DbContext context) : base(context)
        {
        }
    }
}
