using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GeorgiaTech.Messaging
{
    public class ProductProducerMessaging
    {
        public async void ProduceProductMessage()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
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
