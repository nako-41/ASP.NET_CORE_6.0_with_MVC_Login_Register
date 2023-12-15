using ASP.NET_CORE_6._0_with_MVC_Login_Register.Context;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Entities;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Helpers;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Controllers
{
    //[Authorize(Roles = "admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]

    public class UserController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IHasher _hasher;

        public UserController(DatabaseContext databaseContext, IMapper mapper, IHasher hasher)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            _hasher = hasher;
        }

        public IActionResult Index()
        {
            List<User> users = _databaseContext.Users.ToList();

            //  List<UserModel> model = new List<UserModel>();


            // automapper

            List<UserModel> model = users.Select(x => _mapper.Map<UserModel>(x)).ToList();

            //foreach (var item in users)
            //{
            //    model.Add(new UserModel
            //    {
            //        FullName = item.FullName
            //    });
            //}

            // _databaseContext.Users.Select(x => new UserModel { FullName = x.FullName }).ToList();

             
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists");
                    return View(model);
                }

                User user = _mapper.Map<User>(model);

                user.Password = _hasher.DoMD5HashedString(model.Password);

                _databaseContext.Users.Add(user);
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public IActionResult Edit(Guid id)
        {
            User user = _databaseContext.Users.Find(id);

            EditUserModel model = _mapper.Map<EditUserModel>(user);

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Guid id, EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(x => x.Username.ToLower() == model.Username.ToLower() && x.ID != id))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists");
                    return View(model);
                }


                User user = _databaseContext.Users.Find(id);

                _mapper.Map(model, user);

                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Delete(Guid id)
        {
            User user = _databaseContext.Users.Find(id);


            if (user != null)
            {

                _databaseContext.Users.Remove(user);
                _databaseContext.SaveChanges();
            }


            return RedirectToAction(nameof(Index));

        }
    }
}
