using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeorgiaTech.Application.Contracts;
using GeorgiaTech.Domain.Common;
using GeorgiaTech.Product.Application.Contracts.Persistence;
using GeorgiaTech.Product.Domain;
using System.Threading;

namespace GeorgiaTech.Product.Application.Features.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CreateProductCommandHandler> _logger;
        public CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger, IProductRepository productRepository)
        {
            this._productRepository = productRepository;
            this._logger = logger;
        }
        public async Task<Result> Handle(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                Domain.Product product = new Domain.Product(command.ProductId, command.UserId, command.Title);
                await this._productRepository.CreateAsync(product);
                return Result.Ok();
            }
            catch (Exception ex)
            {

                this._logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.FromException(ex));
                
            }
        }
    }
}
