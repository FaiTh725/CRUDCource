using Authorize.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Product.Domain.Contracts.Models.Request;

namespace Authorize.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPatch("[action]")]
        public async Task<IActionResult> UpdateUser(ChangeRoleAccount request)
        {
            var response = await userService.UpdateRoleUser(request);

            return new JsonResult(response);
        }
    }
}
