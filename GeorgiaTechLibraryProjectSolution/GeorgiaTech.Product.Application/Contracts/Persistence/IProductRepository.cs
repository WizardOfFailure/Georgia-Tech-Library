using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeorgiaTech.Application.Contracts.Persistence;
using GeorgiaTech.Product.Domain;
using GeorgiaTech.Domain.Common;
using GeorgiaTech.Product.Application.Features.GetAggregatedByProduct;

//Change this to CreateProduct later

namespace GeorgiaTech.Product.Application.Contracts.Persistence
{
    public interface IProductRepository : IRepository<Domain.Product>
    {
        Task<Result<List<Domain.Product>>> GetUserReviews(int userid);
        Task<Result<List<Domain.Product>>> GetProductReviews(int productid);
        Task<Result<ProductReviewDTO>> GetProductAggregatedReviews(int productid);
        Task<Result<List<Domain.Product>>> GetByUser(int userId);
        Task<Result<List<Domain.Product>>> GetByProduct(int productId);
        Task<Result<ProductReviewDTO>> GetAggregatedByProduct(int productId);
    }
}
