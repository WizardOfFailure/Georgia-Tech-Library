using Dapper;
using Microsoft.Extensions.Logging;
using GeorgiaTech.Data.Persistence;
using GeorgiaTech.Domain.Common;
using GeorgiaTech.Product.Application.Contracts.Persistence;
using GeorgiaTech.Product.Application.Features.GetAggregatedByProduct;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace GeorgiaTech.Product.Persistence
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(DataContext dataContext, ILogger<ProductRepository> logger) : base(TableNames.Product.PRODUCTTABLE, dataContext)
        {
            this._logger = logger;
        }

        public async Task CreateAsync(Domain.Product entity)
        {
            try
            {
                string sql = $"insert into {this.TableName} (productid, userid, title) values (@pid, @uid, @title)";
                using (var connection = dataContext.CreateConnection())
                {
                    await connection.ExecuteAsync(sql, new { pid = entity.ProductId, uid = entity.UserId, title = entity.Title });
                }
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw new Exception("Error in inserting review into database", ex);
            }
        }

        //Remove methods below and change interface
        public Task<Result<ProductReviewDTO>> GetAggregatedByProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<Domain.Product>>> GetByProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<Domain.Product>>> GetByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ProductReviewDTO>> GetProductAggregatedReviews(int productid)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<Domain.Product>>> GetProductReviews(int productid)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<Domain.Product>>> GetUserReviews(int userid)
        {
            throw new NotImplementedException();
        }
    }
}
