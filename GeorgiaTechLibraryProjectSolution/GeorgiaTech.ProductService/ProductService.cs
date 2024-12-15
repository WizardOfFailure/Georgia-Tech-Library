using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GeorgiaTech.Product.Domain;

namespace GeorgiaTech.ProductService
{
    public class ProductService
    {
        public ProductService() { }

        public async Task AddBookAsync(Guid bookId, string title, string author, int quantity)
        {
            // Add book logic here (e.g., save to database)

            // Publish BookAddedEvent
            var bookAddedEvent = new BookAddedEvent
            {
                BookId = bookId,
                Title = title,
                Author = author,
                Quantity = quantity
            };

            var body = JsonSerializer.SerializeToUtf8Bytes(bookAddedEvent);
            _channel.BasicPublish(exchange: "BookExchange", routingKey: "BookAdded", basicProperties: null, body: body);

            Console.WriteLine($"BookAddedEvent published for BookId: {bookId}");
        }
    }
}
