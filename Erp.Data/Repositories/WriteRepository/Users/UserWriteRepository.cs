using Erp.Data.Models.Users;
using Microsoft.EntityFrameworkCore;
using Erp.Data.Repositories.WriteRepositories;
using System.Threading.Tasks;
using Erp.View.Models.Users;
using System.Linq;

namespace Erp.Data.Repositories.WriteRepository.Users
{
    public class UserWriteRepository : BaseWriteRepository<User, int>, IUserWriteRepository
    {
        public UserWriteRepository(DbContext context) : base(context)
        {
        }

        public async Task<LoggedUserView> GetLoggedUserViewByIdAsync(int id)
        {
            return await _dbQuery.Where(x => x.Id == id).Select(x => new LoggedUserView
            {
                Id = x.Id,
                FullName = x.Person.FullName,
                PersonId = x.PersonId
            }).FirstOrDefaultAsync();
        }
    }
}
