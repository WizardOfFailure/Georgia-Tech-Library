using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgiaTech.Product.Application.Features.GetAggregatedByProduct
{
    public class GetAggregatedByProductQuery : IQuery<ProductReviewDTO>
    {
        public GetAggregatedByProductQuery(int productId)
        {
            Ensure.That(productId, nameof(productId)).IsGt<int>(0);
            ProductId = productId;
        }
        public int ProductId { get; private set; }
    }
}
