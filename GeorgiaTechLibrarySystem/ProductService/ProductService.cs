using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ProductService
{
    public class ProductService
    {
        /*public void HandleBookAdded(BookAddedEvent event){

            var productStatus = event.S
        }*/


        public async void ProduceMessage()
        {
            var factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = "user", Password = "password", Port= 5672 };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "Product_Queue", durable: true, exclusive: false, autoDelete: false,
            arguments: null);

            await channel.ExchangeDeclareAsync(exchange: "product_direct_exchange", type: ExchangeType.Direct);

            var message = "Book has been added";
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: "product_direct_exchange", routingKey: "Product_Queue", body: body);
        }

        public async void ProduceBasicMessage()
        {
            var factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = "user", Password = "password", Port = 5672 };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "basicBookQueue", durable: false, exclusive: false, autoDelete: false,
            arguments: null);

            const string message = "Book Added";
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "basicBookQueue", body: body);
            Console.WriteLine($" [x] Sent {message}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public async void ProduceTaskMessage()
        {
            var factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = "user", Password = "password", Port = 5672 };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "book_task_queue", durable: true, exclusive: false,
            autoDelete: false, arguments: null);


            const string message = "Book Added";
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "book_task_queue", body: body);
            Console.WriteLine($" [x] Sent {message}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }

        public async void ProducePublishSubscribeMessage()
        {
            var factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = "user", Password = "password", Port = 5672 };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            //Declare exchange
            await channel.ExchangeDeclareAsync(exchange: "books", type: ExchangeType.Fanout);

            var message = "Book added";
            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(exchange: "books", routingKey: string.Empty, body: body);
            Console.WriteLine($" [x] Sent {message}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public async void ProducePublishSubscribeSagaMessage()
        {
            var factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = "user", Password = "password", Port = 5672 };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            // Declare exchange
            await channel.ExchangeDeclareAsync(exchange: "saga_books", type: ExchangeType.Fanout);

            var message = "Book added";
            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(exchange: "saga_books", routingKey: string.Empty, body: body);
            Console.WriteLine($" [x] Sent {message}");

            // Listen for compensation events
            await channel.ExchangeDeclareAsync(exchange: "saga_compensation", type: ExchangeType.Fanout);
            var compensationQueue = await channel.QueueDeclareAsync();
            await channel.QueueBindAsync(queue: compensationQueue.QueueName, exchange: "saga_compensation", routingKey: string.Empty);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var compensationBody = ea.Body.ToArray();
                var compensationMessage = Encoding.UTF8.GetString(compensationBody);
                Console.WriteLine($" [x] Compensation received: {compensationMessage}");

                // Handle compensation (e.g., remove product)
                if (compensationMessage == "Revert book addition")
                {
                    Console.WriteLine(" [!] Reverting book addition.");
                }

                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(compensationQueue.QueueName, autoAck: true, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
