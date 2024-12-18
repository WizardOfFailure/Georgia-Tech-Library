using Microsoft.AspNetCore.Connections;
using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;

namespace ProductService
{
    public class ProductProduceMessage
    {
        public async void ProduceMessage()
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq" };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "Product_Queue", durable: true, exclusive: false, autoDelete: false,
            arguments: null);

            await channel.ExchangeDeclareAsync(exchange: "product_direct_exchange", type: ExchangeType.Direct);
            var message = "Book has been added";
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: "product_direct_exchange", routingKey: "Product_Queue", body: body);
        }

    }
}
