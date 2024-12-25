using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Domain.Contracts.Models.Product;
using Product.Services.Interfaces;

namespace Product.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        public readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Seller, Admin")]
        public async Task<IActionResult> Upload(UploadProduct product)
        {
            var result = await productService.UploadProduct(product);

            return new JsonResult(result);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> IsProductInCart(long productId, string email)
        {
            var response = await productService.IsProductInCart(productId, email);

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Product(long id)
        {
            var result = await productService.GetProduct(id);

            return new JsonResult(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ProductsPagination(int page, int count)
        {
            var response = await productService.GetProductPagination(page, count);

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Products()
        {
            var response = await productService.GetProducts();

            return new JsonResult(response);
        }
    }
}
