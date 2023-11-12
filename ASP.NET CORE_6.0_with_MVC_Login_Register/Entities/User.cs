using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid ID { get; set; }

        [StringLength(50)]
        public string? FullName { get; set; }

        [Required]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool Locked { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }


}
