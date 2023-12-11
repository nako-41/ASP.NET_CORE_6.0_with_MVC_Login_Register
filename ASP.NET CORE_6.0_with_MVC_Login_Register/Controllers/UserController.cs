using ASP.NET_CORE_6._0_with_MVC_Login_Register.Context;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Entities;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public UserController(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
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
    }
}
