using ASP.NET_CORE_6._0_with_MVC_Login_Register.Helpers;
using ASP.NET_CORE_6._0_with_MVC_Login_Register.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_CORE_6._0_with_MVC_Login_Register.Controllers.API
{
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestoController : ControllerBase
    {
        private readonly IApiTokenHelper _tokenHelper;

        public TestoController(IApiTokenHelper tokenHelper)
        {
            _tokenHelper = tokenHelper;
        }

        [HttpGet]
        public ActionResult GetData([FromBody] PostDataApiModel model) 
        {
            return Ok(model);

        }

        [HttpPost]
        public IActionResult PostData([FromBody] PostDataApiModel model)
        {
            return Ok(model);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            if (model.Username=="aaa" && model.Password=="123123")
            {
                string token = _tokenHelper.GenerateToken(model.Username, 0);

                return Ok(new {Error=false,Message="Token created ",Data=token });
            }
            else
            {
                return BadRequest(new {Error=true,Message="Incorrent Identity"});
            }
        }
    }
}
