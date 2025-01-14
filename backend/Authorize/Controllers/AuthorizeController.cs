using Authorize.Contracts.User;
using Authorize.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Authorize.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthorizeController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserLogin request)
        {
            var response = await authenticationService.Login(request);

            return new JsonResult(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(UserRegister request)
        {
            var response = await authenticationService.Register(request);

            return new JsonResult(response);
        }

        [HttpGet("Verify")]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            var result = await authenticationService.VerifyEmail(email);

            return new JsonResult(result);
        }

        [HttpGet("Confirm")]
        public async Task<IActionResult> ConfirmEmail(string email, int key)
        {
            var result = await authenticationService.ConfirmEmail(email, key);

            return new JsonResult(result);
        }
    }
}
