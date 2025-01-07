using Microsoft.AspNetCore.Connections;
using PrometheusMonitoring;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace InventoryService
{
    public class InventoryService
    {

        //public async void ReceiveMessage()
        //{
        //    var factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = "user", Password = "password", Port = 5672 };
        //    var connection = await factory.CreateConnectionAsync();
        //    var channel = await connection.CreateChannelAsync();

        //    QueueDeclareOk queueDeclareResult = await channel.QueueDeclareAsync(queue: "Product_Queue", durable: true, exclusive: false, autoDelete: false,
        //    arguments: null);

        //    await channel.ExchangeDeclareAsync(exchange: "product_direct_exchange", type: ExchangeType.Direct);

        //   // QueueDeclareOk queueDeclareResult = await channel.QueueDeclareAsync();

        //    string queueName = queueDeclareResult.QueueName;

        //    await channel.QueueBindAsync(queue: queueName, exchange: "product_direct_exchange", routingKey: "Product_Queue");

        //    var consumer = new AsyncEventingBasicConsumer(channel);
        //    consumer.ReceivedAsync += (model, ea) =>
        //    {
        //        byte[] body = ea.Body.ToArray();
        //        var message = Encoding.UTF8.GetString(body);
        //        var routingKey = ea.RoutingKey;
        //        //Console.WriteLine($" [x] {message}");
        //        //Console.WriteLine($" [x] Received '{routingKey}':'{message}'");
        //        return Task.CompletedTask;
        //    };

        //    await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);
        //}

        //public async void ReceiveBasicMessage()
        //{
        //    var factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = "user", Password = "password", Port = 5672 };
        //    var connection = await factory.CreateConnectionAsync();
        //    var channel = await connection.CreateChannelAsync();

        //    await channel.QueueDeclareAsync(queue: "basicBookQueue", durable: false, exclusive: false, autoDelete: false,
        //    arguments: null);

        //    Console.WriteLine(" [*] Waiting for messages.");

        //    var consumer = new AsyncEventingBasicConsumer(channel);
        //    consumer.ReceivedAsync += (model, ea) =>
        //    {
        //        var body = ea.Body.ToArray();
        //        var message = Encoding.UTF8.GetString(body);
        //        Console.WriteLine($" [x] Received {message}");
        //        return Task.CompletedTask;
        //    };

        //    await channel.BasicConsumeAsync("basicBookQueue", autoAck: true, consumer: consumer);

        //    Console.WriteLine(" Press [enter] to exit.");
        //    Console.ReadLine();
        //}

        //public async void ReceiveMessageReturnAck()
        //{
        //    var factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = "user", Password = "password", Port = 5672 };
        //    var connection = await factory.CreateConnectionAsync();
        //    var channel = await connection.CreateChannelAsync();

        //    await channel.QueueDeclareAsync(queue: "book_task_queue", durable: false, exclusive: false, autoDelete: false,
        //    arguments: null);

        //    Console.WriteLine(" [*] Waiting for messages.");

        //    var consumer = new AsyncEventingBasicConsumer(channel);
        //    consumer.ReceivedAsync += (model, ea) =>
        //    {
        //        var body = ea.Body.ToArray();
        //        var message = Encoding.UTF8.GetString(body);
        //        Console.WriteLine($" [x] Received {message}");
        //        return Task.CompletedTask;
        //    };

        //    ulong Tag = 23;

           

        //    await channel.BasicAckAsync(deliveryTag: Tag, multiple: false); //This makes sure if the consumer dies then the task is not lost

        //    await channel.BasicConsumeAsync("book_task_queue", autoAck: false, consumer: consumer);



        //    Console.WriteLine(" Press [enter] to exit.");
        //    Console.ReadLine();
        //}

        public async void ReceivePublishSubscribeMessage()
        {
            var factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = "user", Password = "password", Port = 5672 };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();


            await channel.ExchangeDeclareAsync(exchange: "books",
            type: ExchangeType.Fanout);


            QueueDeclareOk queueDeclareResult = await channel.QueueDeclareAsync();
            string queueName = queueDeclareResult.QueueName;
            await channel.QueueBindAsync(queue: queueName, exchange: "books", routingKey: string.Empty);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public async void ReceivePublishSubscribeSagaMessage()
        {
            var factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = "user", Password = "password", Port = 5672 };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "saga_books", type: ExchangeType.Fanout);
            var inventoryQueue = await channel.QueueDeclareAsync();
            await channel.QueueBindAsync(queue: inventoryQueue.QueueName, exchange: "saga_books", routingKey: string.Empty);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");

                // Simulate inventory update
                bool inventoryUpdated = UpdateInventory(message);

                if (!inventoryUpdated)
                {
                    // Publish compensation event
                    await channel.ExchangeDeclareAsync(exchange: "saga_compensation", type: ExchangeType.Fanout);
                    var compensationMessage = "Revert book addition";
                    var compensationBody = Encoding.UTF8.GetBytes(compensationMessage);
                    await channel.BasicPublishAsync(exchange: "saga_compensation", routingKey: string.Empty, body: compensationBody);
                    Console.WriteLine($" [!] Published compensation: {compensationMessage}");
                }

                await Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(inventoryQueue.QueueName, autoAck: true, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private bool UpdateInventory(string message)
        {
            Console.WriteLine($" [x] Updating inventory for: {message}");

            // Simulate success or failure
            bool isSuccess = new Random().Next(0, 2) == 1;

            if (!isSuccess)
            {
                Console.WriteLine(" [!] Inventory update failed!");
            }
            else
            {
                Console.WriteLine(" [x] Inventory updated successfully.");
            }

            return isSuccess;
        }


    }
}
