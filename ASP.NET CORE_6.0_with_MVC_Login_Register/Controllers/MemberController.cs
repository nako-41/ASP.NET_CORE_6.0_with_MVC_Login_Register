﻿using ASP.NET_CORE_6._0_with_MVC_Login_Register.Context;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Entities;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Helpers;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Controllers
{
    [Authorize(Roles ="admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class MemberController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IHasher _hasher;

        public MemberController(DatabaseContext databaseContext, IMapper mapper, IHasher hasher)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            _hasher = hasher;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MemberListPartial() 
        {
            List<User> users = _databaseContext.Users.ToList();

            List<UserModel> model = users.Select(x => _mapper.Map<UserModel>(x)).ToList();

            return PartialView("_MemberListPartial", model);

        }

        public IActionResult MemberAddPartial()
        {

            return PartialView("_MemberAddPartial", new CreateUserModel());

        }

        [HttpPost]
        public IActionResult AddNewMember(CreateUserModel model)
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
            }

            return PartialView("_MemberAddPartial",model);

        }
    }
}
