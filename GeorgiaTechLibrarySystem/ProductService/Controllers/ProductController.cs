using Microsoft.AspNetCore.Mvc;


namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        //private readonly PerformanceMetricsService _metricsService;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
            //_metricsService = metricsService;
        }
        /*
        public IActionResult Index()
        {
            return View();
        }*/

        //[HttpPost(Name = "PostProduct")]
        /*[HttpPost]
        public IActionResult Post()
        {
            return Accepted(); //Change this
        }*/

        //[HttpPost]
        //[Route("SendMessage")]
        //public IActionResult PostMessage()
        //{
        //    ProductService productService = new ProductService();
        //    productService.ProduceMessage();
        //    return Accepted(); 
        //}

        //[HttpPost]
        //[Route("SendBasicMessage")]
        //public IActionResult PostBasicMessage()
        //{
        //    ProductService productService = new ProductService();
        //    productService.ProduceBasicMessage();
        //    return Accepted(); 
        //}

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
        public IActionResult PostPublishSubsribeSagaMessage([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Invalid JSON payload.");
            }

            ProductService productService = new ProductService();
            productService.ProducePublishSubscribeSagaMessage(book);
            return Accepted();
        }

    }
}
