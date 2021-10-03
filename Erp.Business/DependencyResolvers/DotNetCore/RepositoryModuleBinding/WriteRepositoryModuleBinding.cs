using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using Erp.Data.Repositories.WriteRepository.People;
using Erp.Data.Repositories.WriteRepository.Users;

namespace Erp.Business.DependencyResolvers.DotNetCore.RepositoryModuleBinding
{
    public class WriteRepositoryModuleBinding : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            #region People
            services.AddTransient<IPersonWriteRepository, PersonWriteRepository>();
            services.AddTransient<IPersonEmailAddressWriteRepository, PersonEmailAddressWriteRepository>();
            #endregion

            #region Users
            services.AddTransient<IUserWriteRepository, UserWriteRepository>();
            #endregion
        }
    }
}
