using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ArcherMobilApp.Infrastracture.Security.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ArcherMobilApp.Infrastracture
{
    public class TokenGenerator
    {
        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtToken GenerateToken(string identityValue, IEnumerable<string> identityRoles)
        {
            var identity = GetIdentity(identityValue);

            if (identity == null)
            {
                return null;
            }
            
            
            var now = DateTime.UtcNow;

            var jti = GenerateRefreshToken();

            return new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create(_configuration.GetValue<string>("Jwt:SecurityKey")))
                .AddIssuer(_configuration.GetValue<string>("Jwt:Issuer"))
                .AddAudience(_configuration.GetValue<string>("Jwt:Audience"))
                .AddIdentityName(identityValue)
                .AddRoles(identityRoles)
                .AddExpiry(_configuration.GetValue<int>("Jwt:ExpiredTimeMinutes"))
                .AddClaim(JwtRegisteredClaimNames.Jti, jti)
                .Build();
        }

        private ClaimsIdentity GetIdentity(string email)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, email)
                };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;

        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string tokenBearer)
        {

            if (!string.IsNullOrWhiteSpace(tokenBearer) && tokenBearer.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
                tokenBearer = tokenBearer.Substring(6).TrimStart();


            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:SecurityKey")));
            var tokenValidationParameters = new TokenValidationParameters
            {
                
                ValidIssuer = _configuration.GetValue<string>("Jwt:Issuer"),
                ValidateIssuer = true,
                ValidateAudience = true, //you might want to validate the audience and issuer depending on your use case
                ValidAudience = _configuration.GetValue<string>("Jwt:Audience"),
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(tokenBearer, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }



        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
