using Erp.Data.Models.Users;
using Erp.Data.Repositories.WriteRepositories;
using Erp.View.Models.Users;
using System.Threading.Tasks;

namespace Erp.Data.Repositories.WriteRepository.Users
{
    public interface IUserWriteRepository : IBaseWriteRepository<User, int>
    {
        Task<LoggedUserView> GetLoggedUserViewByIdAsync(int id);
    }
}
