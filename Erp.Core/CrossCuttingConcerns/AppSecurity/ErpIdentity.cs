using System.Security.Principal;

namespace Core.CrossCuttingConcerns.AppSecurity
{
    public class ErpIdentity : IIdentity
    {
        public string User { get; set; }
        public string Name { get; set; }
        public int PersonId { get; set; }
        public string DeviceId { get; set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}