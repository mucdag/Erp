using Erp.Data.Models.People;
using Erp.Data.Repositories.WriteRepositories;

namespace Erp.Data.Repositories.WriteRepository.People
{
    public interface IPersonWriteRepository : IBaseWriteRepository<Person, int>
    {
    }
}
