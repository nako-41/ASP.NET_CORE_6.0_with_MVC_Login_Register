using ASP.NET_CORE_6._0_with_MVC_Login_Register.Entities;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Models.ViewModels;
using AutoMapper;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserModel>().ReverseMap();    
        }
    }
}
