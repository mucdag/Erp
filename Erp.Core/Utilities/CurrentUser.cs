using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using Core.CrossCuttingConcerns.AppSecurity;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Core.Utilities
{
    public class CurrentUser
    {
        public static string User
        {
            get
            {
                if (UserPrincipal.Identity is ErpIdentity identity)
                    return identity.User ?? identity.Name;

                return Thread.CurrentPrincipal?.Identity.Name;
            }
        }

        public static ErpIdentity UserIdentity
        {
            get
            {
                if (UserPrincipal.Identity is ErpIdentity identity)
                    return identity;

                var httpContext = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
                if (httpContext.HttpContext?.User != null)
                {
                    var idntty = httpContext.HttpContext.User.Identity;
                    var personId = httpContext.HttpContext.User.Claims.Where(x => x.Type == "PersonId").FirstOrDefault()?.Value;
                    return new ErpIdentity
                    {
                        Name = idntty.Name,
                        User = httpContext.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault()?.Value,
                        PersonId = personId != null ? Convert.ToInt32(personId) : 0,
                        AuthenticationType = idntty.AuthenticationType,
                        IsAuthenticated = idntty.IsAuthenticated
                    };
                }

                if (Thread.CurrentPrincipal != null)
                {
                    var idntty = Thread.CurrentPrincipal.Identity;
                    return new ErpIdentity
                    {
                        Name = idntty.Name,
                        AuthenticationType = idntty.AuthenticationType,
                        IsAuthenticated = idntty.IsAuthenticated
                    };
                }
                return null;
            }
        }

        public static ErpPrincipal UserPrincipal
        {
            get
            {
                var userPrincipal = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext?.User;
                if (userPrincipal != null)
                    return new ErpPrincipal(userPrincipal.Identity);

                if (Thread.CurrentPrincipal is ErpPrincipal principal)
                    return principal;

                return new ErpPrincipal(Thread.CurrentPrincipal?.Identity);
            }
        }

        public static string UserAgent
        {
            get
            {
                try
                {
                    return ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.Request.Headers["User-Agent"].ToString();
                }
                catch (Exception)
                {
                }

                return string.Empty;
            }
        }

        public static string IPAdresses
        {
            get
            {
                try
                {
                    var httpContext = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
                    return httpContext.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                catch (Exception)
                {
                }

                return string.Empty;
            }
        }

        public static string Language
        {
            get
            {
                try
                {
                    return ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.Request.Headers[HeaderNames.AcceptLanguage];
                }
                catch (Exception)
                {
                }

                return string.Empty;
            }
        }

        public static string RequestId
        {
            get
            {
                try
                {
                    var requestId = StringValues.Empty;
                    ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.Request.Headers.TryGetValue("X-Request-Id", out requestId);
                    return requestId;
                }
                catch (Exception)
                {
                }

                return string.Empty;
            }
        }
    }
}