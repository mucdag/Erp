using System;
using System.Threading.Tasks;

namespace Erp.Business.WriteManagers.Users
{
    public interface IAccountWriteManager
    {
        Task SendPasswordResetEmailCodeAsync(string to, string code, DateTime time);
    }
}
