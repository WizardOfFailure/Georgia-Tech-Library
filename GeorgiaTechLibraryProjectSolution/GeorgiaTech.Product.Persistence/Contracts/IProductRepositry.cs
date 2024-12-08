using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeorgiaTech.Domain.Common;
using GeorgiaTech.Product.Application.Features.GetAggregatedByProduct;
using GeorgiaTech.Product.Domain;

//change this to create product

namespace GeorgiaTech.Product.Persistence.Contracts
{
    public interface IProductRepositry : IBaseRepository<Domain.Product>
    {
        Task<Result<List<Domain.Product>>> GetUserReviews(int userid);
        Task<Result<List<Domain.Product>>> GetProductReviews(int productid);
        Task<Result<ProductReviewDTO>> GetProductAggregatedReviews(int productid);
    }

    public interface IBaseRepository<T>
    {
        Task<Result<T>> GetByIdAsync(int id);
        Task<Result> AddAsync(T entity);
        Task<Result> UpdateAsync(T entity);
        Task<Result> DeleteAsync(int id);
    }
}
