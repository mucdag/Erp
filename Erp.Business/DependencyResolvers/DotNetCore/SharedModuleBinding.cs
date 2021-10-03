using Core.Utilities.BlobServices.Concrete;
using Core.Utilities.BlobServices.Interfaces;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Erp.Business.DependencyResolvers.DotNetCore
{
    public class SharedModuleBinding : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            // Others
            services.AddScoped<IBlobService, BlobFileSystemService>();
        }
    }
}