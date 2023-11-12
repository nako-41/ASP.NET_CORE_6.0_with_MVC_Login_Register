using ASP.NET_CORE_6._0_with_MVC_Login_Register.Context;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Entities;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Models;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;

        public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //login islemleri
            }

            return View(model);
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string md5salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                string saltedpassword = model.Password + md5salt;
                string hashedpass = saltedpassword.MD5();

                User user = new()
                {
                    Username=model.Username,
                    Password=hashedpass
                };

                _databaseContext.Users.Add(user);
                _databaseContext.SaveChanges();
            }
            return View(model);
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
