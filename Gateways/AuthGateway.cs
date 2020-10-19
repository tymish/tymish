using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Gateways
{
    public class AuthGateway : IAuthGateway
    {
        private readonly AuthOptions _options;
        public AuthGateway(IOptions<AuthOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateVendorToken(Vendor vendor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("sub", vendor.Id.ToString()),
                    new Claim(ClaimTypes.Email, vendor.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(_options.ExpiryDays),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = SignToken(_options.Secret)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private SigningCredentials SignToken(string secret)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            return new SigningCredentials(
                mySecurityKey,
                SecurityAlgorithms.HmacSha256Signature);
        }
    }
}