using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestSystem.API.Core.DTO;
using TestSystem.API.Core.Services.Interfaces;

namespace TestSystem.API.Core.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [EnableCors]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var info = await _identityService.RegisterAsync(userDto);
            return Ok(info);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credentials cred)
        {
            var info = await _identityService.LoginAsync(cred);
            return Ok(info);
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
        {
            await _identityService.UpdateUserAsync(userDto);
            return Ok();
        }

        [HttpPost("password")]
        public async Task<IActionResult> ChangePassword([FromBody] Credentials cred)
        {
            var response = await _identityService.ChangePasswordAsync(cred);
            return Ok(response);
        }
    }
}
