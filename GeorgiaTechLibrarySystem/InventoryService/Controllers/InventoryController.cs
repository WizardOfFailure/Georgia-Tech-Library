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
