using System.ComponentModel.DataAnnotations;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Models.ViewModels
{
    public class UserModel
    {
   
        public Guid ID { get; set; }
        public string? FullName { get; set; }
        public string Username { get; set; }
        public bool Locked { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string Role { get; set; } = "user";
    }
}
