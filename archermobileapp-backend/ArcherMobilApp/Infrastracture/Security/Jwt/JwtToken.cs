using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace ArcherMobilApp.Infrastracture.Security.Jwt
{
    public sealed class JwtToken
    {
        private JwtSecurityToken _token;

        internal JwtToken(JwtSecurityToken token)
        {
            this._token = token;
        }

        public DateTime ValidTo => _token.ValidTo;
        public string Jti => _token.Claims?.Where(obj => obj.Type == JwtRegisteredClaimNames.Jti).FirstOrDefault()?.Value;
        public string Value => new JwtSecurityTokenHandler().WriteToken(this._token);
    }
}
