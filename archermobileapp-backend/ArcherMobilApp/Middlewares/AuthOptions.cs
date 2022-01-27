using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ArcherMobilApp.Middlewares
{
    public class AuthOptions
    {
        public const string ISSUER = "ArcherServer"; // издатель токена
        public const string AUDIENCE = "ArcherMobileClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 1; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
