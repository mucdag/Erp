using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Erp.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Erp.Business.WriteManagers.Users
{
    public class AccountWriteManager : IAccountWriteManager
    {
        IHttpContextAccessor _httpContext;

        public AccountWriteManager(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public async Task SendPasswordResetEmailCodeAsync(string to, string code, DateTime time)
        {
            var domain = new StringValues();
            _httpContext.HttpContext.Request.Headers.TryGetValue("origin", out domain);
            var url = $"{domain}/#/hesap-yonetimi/sifre-yenile/{to}/{code}";
            var body = url;
            body += "\n" + time.ToString();
            await Task.Run(() => EmailHelper.SendEmail(to, body, "ERP Sistem Şifre Sıfırlama"));
        }

    }
}