using Core.CrossCuttingConcerns.AppSecurity;
using Core.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Core.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly int expiryinDays;
        private readonly string jwtSecurityKey;
        private readonly string jwtIssuer;
        public TokenService(IOptions<AppSettings> options)
        {
            expiryinDays = options.Value.JwtExpiryinDays;
            jwtSecurityKey = options.Value.JwtSecurityKey;
            jwtIssuer = options.Value.JwtIssuer;
        }

        public string BuildToken(ErpIdentity userData)
        {
            var claims = new[]
            {
                new Claim("Name", userData.Name),
                new Claim("User", userData.User),
                new Claim("PersonId", userData.PersonId.ToString()),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(jwtIssuer, jwtIssuer, claims, expires: DateTime.Now.AddDays(expiryinDays), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
