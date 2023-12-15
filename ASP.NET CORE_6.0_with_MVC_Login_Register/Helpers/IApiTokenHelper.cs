namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Helpers
{
    public interface IApiTokenHelper
    {
        string GenerateToken(string username, int id);
    }
}