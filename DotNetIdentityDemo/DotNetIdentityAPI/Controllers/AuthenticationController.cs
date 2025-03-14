using DotNetIdentityAPI.Services;
using DotNetIdentityShared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetIdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
             _userService = userService;
        }

        //Path : api/Authentication/Register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                var serviceResponse = await _userService.RegisterUserAsync(registerDTO);
                if (serviceResponse.IsSuccess)
                {
                    return Ok(serviceResponse);
                }
                return BadRequest(serviceResponse);  
            }
            catch (Exception ex)
            {
                return BadRequest(new UserManagerResponse() { IsSuccess = false, Result = null, Message = ex.Message });

            }
        }
    }
}
