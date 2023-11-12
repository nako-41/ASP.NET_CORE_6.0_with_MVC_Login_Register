using System.ComponentModel.DataAnnotations;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, ErrorMessage = "Username can be max 30 charecters.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password can be min 6 charecters.")]
        [MaxLength(16, ErrorMessage = "Password can be mac 16 charecters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "RePassword is required.")]
        [MinLength(6, ErrorMessage = "RePassword can be min 6 charecters.")]
        [MaxLength(16, ErrorMessage = "RePassword can be mac 16 charecters.")]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}
