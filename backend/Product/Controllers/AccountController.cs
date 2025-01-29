using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Domain.Contracts.Models.Account;
using Product.Domain.Contracts.Models.Request;
using Product.Services.Interfaces;

namespace Product.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> AccountInfo(string email)
        {
            var resposne = await accountService.GetAccountDetails(email);

            return new JsonResult(resposne);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> AccountOrderHistory(string email)
        {
            var response = await accountService.GetAccountOrderHistory(email);

            return new JsonResult(response);
        }

        [HttpPatch("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangeRole(ChangeRoleAccount request)
        {
            var response = await accountService.CreateRequestChangeRole(request);

            return new JsonResult(response);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CommitRequest(CommitRequestChangeRole request)
        {
            var response = await accountService.CommitRequest(request);

            return new JsonResult(response);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> AddToCartProduct(AccountWithProductCountRequest request)
        {
            var response = await accountService.AddProductToCart(request);

            return new JsonResult(response);
        }

        [HttpDelete("[action]")]
        [Authorize]
        public async Task<IActionResult> DeleteProductFromCart(AccountWithProductRequest request)
        {
            var response = await accountService.DeleteProductFromCart(request);

            return new JsonResult(response);    
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAccountCart(string email)
        {
            var response = await accountService.GetAccountCartItems(email);

            return new JsonResult(response);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> BuyProducts(AccountBuyProductRequest request)
        {
            var response = await accountService.BuyProducts(request);

            return new JsonResult(response);
        }
    }
}
