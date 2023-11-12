using ASP.NET_CORE_6._0_with_MVC_Login_Register.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }


}
