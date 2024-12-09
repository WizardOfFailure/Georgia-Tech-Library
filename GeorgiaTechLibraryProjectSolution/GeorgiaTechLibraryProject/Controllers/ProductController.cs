using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GeorgiaTech.Application.Contracts;
using GeorgiaTech.Domain.Common;
using GeorgiaTechLibraryProject.Models.Requests;
using GeorgiaTechLibraryProject.Utilities;
using GeorgiaTech.Product.Application.Features.CreateProduct;
using GeorgiaTech.Messaging;


//Add everything from GeorgiaTech.Product.API

namespace GeorgiaTechLibraryProject.Controllers
{
    [Route("api/addbook")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IDispatcher dispatcher;
        private readonly ILogger<ProductController> logger;
        private readonly InstanceHelper instanceHelper;
        
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            try
            {
                //create and dispatch the command
                CreateProductCommand command = new CreateProductCommand(request.ProductId, request.UserId, request.Title);
                var commandResult = await this.dispatcher.Dispatch(command);
                //send message here

                //Need to put the messaging at service level instead of controller level
                ProductProducerMessaging productMessage = new ProductProducerMessaging();
                productMessage.ProduceProductMessage();

                return FromResult(commandResult);


            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
