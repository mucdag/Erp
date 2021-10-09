using Core.CrossCuttingConcerns.AppSecurity;

namespace Erp.Core.TokenService
{
    public interface ITokenService
    {
        string BuildToken(ErpIdentity userData);
    }
}
