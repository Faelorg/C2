using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.src.Controllers
{
    [Route("/")]
    [ApiController]
    public class UnauthorizeController : ControllerBase{

        readonly IUnauthorize _service;
        public UnauthorizeController(IUnauthorize service)
        {
            this._service = service;
        }

        [HttpPost("/auth")]
        public async Task<ActionResult> Authorize(UserModel userModel){
            var response = await _service.Auth(userModel.email, userModel.password);
            if (response.code != 0)
            {
                return Ok(response.message);                
            }

            return Unauthorized(response.message);
        }
    
        [HttpPost("/reg")]
        public async Task<ActionResult> Register(RegUserModel userModel){
            var response = await _service.Register(userModel);

            if (response.code == 1)
            {
                return Ok(response.message);   
            }

            return Unauthorized(response.message);
        }
    }
}