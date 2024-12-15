using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : Controller
    {
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
            return Ok();
        }
    }
}
