using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Helpers
{
    public interface IApiTokenHelper1
    {
        string GenerateToken(string username, int id);
    }

    public class ApiTokenHelper : IApiTokenHelper1
    {

        private readonly IConfiguration _configuration;

        public ApiTokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username, int id)
        {
            string secret = _configuration.GetValue<string>("AppSettings:Secret");
            byte[] key = Encoding.UTF8.GetBytes(secret);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "admin"),
                new Claim("username",username),
                new Claim("userid",id.ToString())
            };

            JwtSecurityToken jwtSecurityToken =
                new JwtSecurityToken(signingCredentials: credentials, claims: claims, expires: DateTime.Now.AddDays(7));

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }
    }
}
