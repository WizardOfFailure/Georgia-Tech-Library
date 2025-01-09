using Microsoft.AspNetCore.Mvc;


namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;

        }

        [HttpPost]
        [Route("SendPublishSubscribeMessage")]
        public IActionResult PostPublishSubsribeMessage()
        {
            ProductService productService = new ProductService();
            productService.ProducePublishSubscribeMessage();
            return Accepted(); 
        }

        [HttpPost]
        [Route("SendPublishSubscribeSagaMessage")]
        public IActionResult PostPublishSubsribeSagaMessage()
        {
            ProductService productService = new ProductService();
            productService.ProducePublishSubscribeSagaMessage();
            return Accepted();
        }

    }
}
