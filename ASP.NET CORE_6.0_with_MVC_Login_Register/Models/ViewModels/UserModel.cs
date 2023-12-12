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
    public class CreateUserModel
    {
        [Required()]
        [StringLength(50)]
        public string Username { get; set; }

        [Required()]
        [StringLength(50)]
        public string Fullname { get; set; }

        public bool Locked { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password can be min 6 charecters.")]
        [MaxLength(16, ErrorMessage = "Password can be mac 16 charecters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "RePassword is required.")]
        [MinLength(6, ErrorMessage = "RePassword can be min 6 charecters.")]
        [MaxLength(16, ErrorMessage = "RePassword can be mac 16 charecters.")]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }

        [Required]
        [StringLength(30)]
        public string Role { get; set; } = "user";
    }
    public class EditUserModel
    {
        [Required()]
        [StringLength(50)]
        public string Username { get; set; }

        [Required()]
        [StringLength(50)]
        public string Fullname { get; set; }

        public bool Locked { get; set; }

        [Required]
        [StringLength(30)]
        public string Role { get; set; } = "user";
    }
}
