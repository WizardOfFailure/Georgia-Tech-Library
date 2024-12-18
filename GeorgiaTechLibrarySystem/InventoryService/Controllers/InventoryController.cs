using Microsoft.AspNetCore.Mvc;
using PrometheusMonitoring;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : Controller
    {
        private readonly PerformanceMetricsService _metricsService;
        public InventoryController(PerformanceMetricsService metricsService)
        {
            _metricsService = metricsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("ReceiveMessage")]
        public IActionResult Get()
        {
            InventoryService inventoryService = new InventoryService();
            inventoryService.ReceiveMessage();
            return Ok();
        }


        [HttpGet]
        [Route("ReceiveBasicMessage")]
        public IActionResult GetBasicBook()
        {
            InventoryService inventoryService = new InventoryService();
            inventoryService.ReceiveBasicMessage();
            return Ok();
        }


        [HttpGet]
        [Route("ReceivePublishSubscribeMessage")]
        public IActionResult GetPublishSubsribeBook()
        {
            InventoryService inventoryService = new InventoryService();
            inventoryService.ReceivePublishSubscribeMessage();
            return Ok();
        }

        [HttpGet]
        [Route("ReceivePublishSubscribeSagaMessage")]
        public IActionResult GetPublishSubsribeSagaBook()
        {
            InventoryService inventoryService = new InventoryService();
            inventoryService.ReceivePublishSubscribeSagaMessage();
            _metricsService.RecordRequest();
            return Ok();
        }
    }
}
