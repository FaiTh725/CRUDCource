using Microsoft.AspNetCore.Mvc;
using Product.Services.Interfaces;

namespace Product.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ITelemetryService telemetryService;
        private readonly ILogger<TestController> logger;

        public TestController(
            ITelemetryService telemetryService, 
            ILogger<TestController> logger)
        {
            this.telemetryService = telemetryService;
            this.logger = logger;
        }


        [HttpGet("[action]")]
        public IActionResult TestAddProductMetrics()
        {
            telemetryService.RecordProductBought(1, 2);
            telemetryService.RecordProductBought(2, 2);

            return Ok("Product bought");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> TestGet(long productId)
        {
            var result = await telemetryService.GetProductCountMetric(productId);
            var result1 = await telemetryService.GetUserTransaction(1);
            
            if(result.IsFailure)
            {
                logger.LogInformation(result.Error);
            }

            if (result1.IsSuccess)
            {
                logger.LogInformation(result1.Value.ToString());
            }
            else
            {
                logger.LogInformation(result1.Error);
            }

            return Ok(result.IsFailure ?
                "erorr check logs" :
                result.Value);
        }
    }
}
