using NETCore.Encrypt.Extensions;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Helpers
{
    public interface IHasher
    {
        string DoMD5HashedString(string s);
    }

    public class Hasher : IHasher
    {
        private readonly IConfiguration _configuration;

        public Hasher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string DoMD5HashedString(string s)
        {
            string md5salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
            string salted = s + md5salt;
            string hashed = salted.MD5();
            return hashed;
        }
    }
}
