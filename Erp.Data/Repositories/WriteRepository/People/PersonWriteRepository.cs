using Erp.Data.Models.People;
using Microsoft.EntityFrameworkCore;
using Erp.Data.Repositories.WriteRepositories;

namespace Erp.Data.Repositories.WriteRepository.People
{
    public class PersonWriteRepository : BaseWriteRepository<Person, int>, IPersonWriteRepository
    {
        public PersonWriteRepository(DbContext context) : base(context)
        {
        }
    }
}
