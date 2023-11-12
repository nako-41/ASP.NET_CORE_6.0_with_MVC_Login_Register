using ASP.NET_CORE_6._0_with_MVC_Login_Register.Context;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Entities;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NETCore.Encrypt.Extensions;
using System.Security.Claims;

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
                string md5salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                string saltedpassword = model.Password + md5salt;
                string hashedpass = saltedpassword.MD5();

                User user = _databaseContext.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password == hashedpass);
                
                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError(nameof(model.Username), "User is locked ");
                        return View(model);
                    }



                }
                else
                {
                    ModelState.AddModelError("","Username or password is incorrect.");
                }

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("Id", user.ID.ToString()));
                claims.Add(new Claim("Fullname", user.FullName ?? string.Empty));
                claims.Add(new Claim("Username", user.Username));



                ClaimsIdentity identity=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);   

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);


                return RedirectToAction("Index","Home");


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
                if (_databaseContext.Users.Any(x=>x.Username.ToLower()==model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username),"Username is already exists.");
                    View(model);
                }


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
