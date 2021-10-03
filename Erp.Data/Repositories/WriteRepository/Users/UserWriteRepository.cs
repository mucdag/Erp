using Erp.Data.Models.Users;
using Microsoft.EntityFrameworkCore;
using Erp.Data.Repositories.WriteRepositories;

namespace Erp.Data.Repositories.WriteRepository.Users
{
    public class UserWriteRepository : BaseWriteRepository<User, int>, IUserWriteRepository
    {
        public UserWriteRepository(DbContext context) : base(context)
        {
        }
    }
}
