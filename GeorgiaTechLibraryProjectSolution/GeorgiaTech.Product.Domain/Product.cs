using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsureThat;
using GeorgiaTech.Domain.Common;

namespace GeorgiaTech.Product.Domain
{
    public class Product : AggregateRoot
    {
        public Product() { } //for Dapper only
        public Product(int productId, int userId, string title)
        {
            Ensure.That(productId).IsGt(0);
            Ensure.That(userId).IsGt(0);
            Ensure.That(title).IsNotNullOrEmpty();

            ProductId = productId;
            UserId = userId;
            Title = title;
            //Created = System.DateTime.Now;
        }
        public int ProductId { get; private set; }
        public int UserId { get; private set; }
        public string Title { get; private set; } = string.Empty;

    }
}
