using System.Security.Principal;

namespace Core.CrossCuttingConcerns.AppSecurity
{
    public class ErpPrincipal : IPrincipal
    {

        public ErpPrincipal(IIdentity byzIdentity)
        {
            Identity = byzIdentity;
        }

        public IIdentity Identity { get; }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}