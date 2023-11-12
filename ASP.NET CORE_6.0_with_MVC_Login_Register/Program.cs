using ASP.NET_CORE_6._0_with_MVC_Login_Register.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<DatabaseContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services
                 .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(opt =>
                 {
                     opt.Cookie.Name = "abc";
                     opt.ExpireTimeSpan=TimeSpan.FromDays(7);
                     opt.SlidingExpiration = true;
                     opt.LoginPath = "/Account/Login";
                     opt.LogoutPath = "/Account/Logout";
                     opt.AccessDeniedPath = "/Home/AccessDenied";
                 });

            var app = builder.Build();


            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
             
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}