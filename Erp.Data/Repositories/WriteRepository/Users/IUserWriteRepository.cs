using Erp.Data.Models.Users;
using Erp.Data.Repositories.WriteRepositories;

namespace Erp.Data.Repositories.WriteRepository.Users
{
    public interface IUserWriteRepository : IBaseWriteRepository<User, int>
    {
    }
}
