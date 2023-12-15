using ASP.NET_CORE_6._0_with_MVC_Login_Register.Context;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Entities;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Helpers;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Server.HttpSys;
using NETCore.Encrypt.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;



namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[Authorize(AuthenticationSchemes = "Cookies")]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;
        private readonly IHasher _hasher;

        public AccountController(DatabaseContext databaseContext, IConfiguration configuration, IHasher hasher)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
            _hasher = hasher;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]

        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedpass = _hasher.DoMD5HashedString(model.Password);

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
                    ModelState.AddModelError("", "Username or password is incorrect.");
                }

                List<Claim> claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? string.Empty));
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
                claims.Add(new Claim("Username", user.Username));

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]

        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists.");
                    View(model);
                }

                //string md5salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                //string saltedpassword = model.Password + md5salt;
                //string hashedpass = saltedpassword.MD5();

                string hashedpass = _hasher.DoMD5HashedString(model.Password);


                User user = new()
                {
                    Username = model.Username,
                    Password = hashedpass
                };

                _databaseContext.Users.Add(user);
                _databaseContext.SaveChanges();

            }
            return View(model);
        }

        public IActionResult Profile()
        {
            ProfileInfoLoader();

            return View();
        }

        private void ProfileInfoLoader()
        {
            Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _databaseContext.Users.FirstOrDefault(x => x.ID == userid);

            ViewData["fullname"] = user.FullName;
            ViewData["ProfileImage"] = user.ProfileImageFileName;

        }

        [HttpPost]
        public IActionResult ProfileChangeFullName([Required][StringLength(50)] string? fullname)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.FirstOrDefault(x => x.ID == userid);

                user.FullName = fullname;
                _databaseContext.SaveChanges();

                ViewData["fullname"] = "fullname";

                return RedirectToAction(nameof(Profile));
            }

            return View("Profile");
        }

        [HttpPost]
        public IActionResult ProfileChangePassword([Required][MinLength(6)][MaxLength(16)] string? password)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.FirstOrDefault(x => x.ID == userid);

                string hashedpassword = _hasher.DoMD5HashedString(password);

                user.Password = hashedpassword;
                _databaseContext.SaveChanges();

                ViewData["result"] = "PasswordChanged";

                //return RedirectToAction(nameof(Profile));
            }

            ProfileInfoLoader();
            return View("Profile");
        }

        [HttpPost]
        public IActionResult ProfileChangeImage([Required] IFormFile file)
        {
            if (ModelState.IsValid)
            {

                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.FirstOrDefault(x => x.ID == userid);

                string filename = $"p_{userid}.jpg";

                Stream stream = new FileStream($"wwwroot/uploads/{filename}", FileMode.OpenOrCreate);


                file.CopyTo(stream);

                stream.Close();
                stream.Dispose();

                user.ProfileImageFileName = filename;
                _databaseContext.SaveChanges();


                return RedirectToAction(nameof(Profile));
            }

            return View("Profile");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
