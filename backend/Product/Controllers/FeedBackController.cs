using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Domain.Contracts.Models.FeedBack;
using Product.Services.Interfaces;

namespace Product.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackService feedBackService;

        public FeedBackController(IFeedBackService feedBackService)
        {
            this.feedBackService = feedBackService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ProductFeedBacks(long productId)
        {
            var response = await feedBackService.GetFeedBacksProduct(productId);

            return new JsonResult(response);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> AddFeedBack(FeedBackAddRequest request)
        {
            var response = await feedBackService.UploadFeedBack(request);

            return new JsonResult(response);
        }
    }
}
