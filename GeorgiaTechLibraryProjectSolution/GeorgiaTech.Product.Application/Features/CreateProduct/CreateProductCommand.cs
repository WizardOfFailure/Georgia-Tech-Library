using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeorgiaTech.Application.Contracts;

namespace GeorgiaTech.Product.Application.Features.CreateProduct
{
    public class CreateProductCommand : ICommand
    {
        public CreateProductCommand(int productId, int userId, string title)
        {
            Ensure.That(productId).IsGt(0);
            Ensure.That(userId).IsGt(0);
            Ensure.That(title).IsNotNullOrEmpty();
            ProductId = productId;
            Title = title;

        }
        public int ProductId { get; private set; }
        public int UserId { get; private set; }
        public string Title { get; private set; }

    }
}
