using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FestoVideoStream.Options
{
    public static class AuthOptions
    {
        public const string ISSUER = "http://localhost:5000/"; // издатель токена
        public const string AUDIENCE = "http://localhost:5000/"; // потребитель токена
        
        public const int LIFETIME = 10; // время жизни токена - 10 минут

        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации

        public static SymmetricSecurityKey SymmetricSecurityKey =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));

        public static SigningCredentials SigningCredentials =>
            new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
    }
}