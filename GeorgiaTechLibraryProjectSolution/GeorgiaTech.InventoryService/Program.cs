using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

var factory = new ConnectionFactory { HostName = "rabbitmq" };
var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "Product_Queue", durable: true, exclusive: false, autoDelete: false,
arguments: null);

await channel.ExchangeDeclareAsync(exchange: "product_direct_exchange", type: ExchangeType.Direct);

QueueDeclareOk queueDeclareResult = await channel.QueueDeclareAsync();
string queueName = queueDeclareResult.QueueName;

await channel.QueueBindAsync(queue: queueName, exchange: "product_direct_exchange", routingKey: "Product_Queue");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    byte[] body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    //Console.WriteLine($" [x] {message}");
    //Console.WriteLine($" [x] Received '{routingKey}':'{message}'");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);