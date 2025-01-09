using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace ProductService
{
    public class ProductService
    {
        private const string ConnectionString = "Server=productservice-mssql_server,1433;Database=Product;User Id=sa;Password=DpA0NU70m!p-ia3;Encrypt=false;";

        public async Task SaveBookToDatabaseAsync(Book book)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    var query = "INSERT INTO Book (Title, Author, Quantity) VALUES (@Title, @Author, @Quantity)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", book.Title);
                        command.Parameters.AddWithValue("@Author", book.Author);
                        command.Parameters.AddWithValue("@Quantity", book.Quantity);

                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("Book saved to database successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the book to the database: {ex.Message}");
            }
        }

        public async Task DeleteBookFromDatabaseAsync(Book book)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    var query = "DELETE FROM Book WHERE Id = (SELECT TOP 1 Id FROM Book ORDER BY Id DESC)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Last inserted book deleted from database successfully.");
                        }
                        else
                        {
                            Console.WriteLine("No books found to delete.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the last inserted book from the database: {ex.Message}");
            }
        }

        public async void ProducePublishSubscribeMessage()
        {
            var factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = "user", Password = "password", Port = 5672 };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();


            await channel.ExchangeDeclareAsync(exchange: "books", type: ExchangeType.Fanout);

            var message = "Book added";
            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(exchange: "books", routingKey: string.Empty, body: body);
            Console.WriteLine($" [x] Sent {message}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }


        public async void ProducePublishSubscribeSagaMessage(Book book)
        {
            var factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = "user", Password = "password", Port = 5672 };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();


            await channel.ExchangeDeclareAsync(exchange: "saga_books", type: ExchangeType.Fanout);


            await SaveBookToDatabaseAsync(book);


            var message = JsonSerializer.Serialize(book);
            var body = Encoding.UTF8.GetBytes(message);


            await channel.BasicPublishAsync(exchange: "saga_books", routingKey: string.Empty, body: body);
            Console.WriteLine($" [x] Sent book: {message}");


            await channel.ExchangeDeclareAsync(exchange: "saga_compensation", type: ExchangeType.Fanout);
            var compensationQueue = await channel.QueueDeclareAsync();
            await channel.QueueBindAsync(queue: compensationQueue.QueueName, exchange: "saga_compensation", routingKey: string.Empty);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var compensationBody = ea.Body.ToArray();
                var compensationMessage = Encoding.UTF8.GetString(compensationBody);
                Console.WriteLine($" [x] Compensation received: {compensationMessage}");


                if (compensationMessage == "Revert book addition")
                {
                    Console.WriteLine($" [!] Reverting addition of book: {JsonSerializer.Serialize(book)}");
                    await DeleteBookFromDatabaseAsync(book);

                }

                await Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(compensationQueue.QueueName, autoAck: true, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

    }
}
