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
        public async Task<IActionResult> Login(CreateUser request)
        {
            var response = await authenticationService.Login(request);

            if(response.Data != string.Empty)
            {
                Response.Cookies.Append("token", response.Data);
            }

            return new JsonResult(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(CreateUser request)
        {
            var response = await authenticationService.Register(request);

            if (response.Data != string.Empty)
            {
                Response.Cookies.Append("token", response.Data);
            }

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
