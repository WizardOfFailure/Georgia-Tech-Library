using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.ObjectPool;
using System.Text.Json;

namespace InventoryService
{
    public class InventoryService
    {

        private const string ConnectionString = "Server=inventoryservice-mssql_server,1433;Database=Inventory;User Id=sa;Password=DpA0NU70m!p-ia2;Encrypt=false;";

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

                //Json deserializer
                Book deserializedBook = BookDeserializer(message);

                await SaveBookToDatabaseAsync(deserializedBook);





                bool inventoryUpdated = UpdateInventory(message);

                if (!inventoryUpdated)
                {
                    await DeleteBookFromDatabaseAsync(deserializedBook);

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

        public Book BookDeserializer(string book)
        {
            Book books = JsonSerializer.Deserialize<Book>(book);
            
            if (books == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            else
            {
                return books;
            }
        }

        private bool UpdateInventory(string message)
        {
            Console.WriteLine($" [x] Updating inventory for: {message}");

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
