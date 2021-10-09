using System.Threading.Tasks;
using Erp.Resource.Models.Users;
using Erp.View.Models.Users;

namespace Erp.Business.WriteManagers.Users
{
    public interface IUserWriteManager
    {
        Task AddAsync(UserResource resource);
        Task UpdateAsync(UserResource resource);
        Task PasswordResetRequestAsync(PasswordResetResource passwordResetResource);
        Task PasswordResetAsync(PasswordResetResource passwordResetResource);
        Task PasswordChangeAsync(PasswordChangeResource passwordChangeResource);
        Task<LoggedUserView> LoginAsync(UserConfirmResource userResource);
    }
}
