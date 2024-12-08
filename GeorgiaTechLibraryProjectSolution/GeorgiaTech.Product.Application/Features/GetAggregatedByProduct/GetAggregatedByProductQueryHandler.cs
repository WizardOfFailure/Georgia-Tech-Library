using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeorgiaTech.Application.Contracts;
using GeorgiaTech.Domain.Common;
using GeorgiaTech.Product.Application.Contracts.Persistence;
using System.Threading;

namespace GeorgiaTech.Product.Application.Features.GetAggregatedByProduct
{
    public class GetAggregatedByProductQueryHandler : IQueryHandler<GetAggregatedByProductQuery, ProductReviewDTO>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetAggregatedByProductQueryHandler> _logger;

        public GetAggregatedByProductQueryHandler(ILogger<GetAggregatedByProductQueryHandler> logger, IProductRepository productRepository)
        {
            this._logger = logger;
            this._productRepository = productRepository;
        }
        public async Task<Result<ProductReviewDTO>> Handle(GetAggregatedByProductQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                return await this._productRepository.GetAggregatedByProduct(query.ProductId);
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                return Result.Fail<ProductReviewDTO>(Errors.General.FromException(ex));
            }
        }
    }
}
