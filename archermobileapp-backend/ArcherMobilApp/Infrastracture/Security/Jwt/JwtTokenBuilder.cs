using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace ArcherMobilApp.Infrastracture.Security.Jwt
{
    public sealed class JwtTokenBuilder
    {
        private SecurityKey _securityKey = null;
        private string _identityName = null;
        private List<string> _identityRoles = new List<string>();
        private string _issuer = "";
        private string _audience = "";
        private readonly Dictionary<string, string> _claims = new Dictionary<string, string>();
        private int _expiryInMinutes = 5;
        private DateTime? _notBefore = DateTime.UtcNow;

        public JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
        {
            this._securityKey = securityKey;
            return this;
        }

        public JwtTokenBuilder AddIdentityName(string identityName)
        {
            this._identityName = identityName;
            return this;
        }

        public JwtTokenBuilder AddRoles(IEnumerable<string> identityRoles)
        {
            this._identityRoles.AddRange(identityRoles);
           return this;
        }

        public JwtTokenBuilder AddIssuer(string issuer)
        {
            this._issuer = issuer;
            return this;
        }

        public JwtTokenBuilder AddAudience(string audience)
        {
            this._audience = audience;
            return this;
        }

        public JwtTokenBuilder AddClaim(string type, string value)
        {
            this._claims.Add(type, value);
            return this;
        }

        public JwtTokenBuilder AddClaims(Dictionary<string, string> claims)
        {
            this._claims.Union(claims);
            return this;
        }

        public JwtTokenBuilder AddExpiry(int expiryInMinutes)
        {
            this._expiryInMinutes = expiryInMinutes;
            return this;
        }
        
        public JwtTokenBuilder AddNotBefore(DateTime notBefore)
        {
            this._notBefore = notBefore;
            return this;
        }

        public JwtToken Build()
        {
            EnsureArguments();
            var handler = new JwtSecurityTokenHandler();

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(_identityName, "TokenAuth"),
                    new[] {
                    new Claim(_identityName, "")
                }.Union(_claims.Select(item => new Claim(item.Key, item.Value)))
                .Union(_identityRoles.Select(item => new Claim(ClaimTypes.Role, item))).ToArray()
            );

            var securityToken = handler.CreateJwtSecurityToken(new SecurityTokenDescriptor()
            {
                Issuer = this._issuer,
                Audience = this._audience,
                SigningCredentials = new SigningCredentials(this._securityKey, SecurityAlgorithms.HmacSha256),
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(_expiryInMinutes),
                NotBefore = _notBefore ?? DateTime.UtcNow
            });

            return new JwtToken(securityToken);
        }

        private void EnsureArguments()
        {
            if (this._securityKey == null)
                throw new ArgumentNullException("SecurityKey");

            if (string.IsNullOrEmpty(this._issuer))
                throw new ArgumentNullException("Issuer");

            if (string.IsNullOrEmpty(this._audience))
                throw new ArgumentNullException("Audience");

            if (string.IsNullOrEmpty(this._identityName))
                throw new ArgumentNullException("IdentityName");
        }
    }
}
