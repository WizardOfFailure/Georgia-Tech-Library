using Microsoft.Data.SqlClient;
using Moq;
using ProductService;
using System.Threading.Tasks;
using Xunit;

public class ProductServiceTests
{
	[Fact]
	public async Task SaveBookToDatabaseAsync_ShouldCallExecuteNonQueryAsync()
	{
		var mockSqlConnection = new Mock<SqlConnection>();
		var mockSqlCommand = new Mock<SqlCommand>();
		mockSqlCommand.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

		var productService = new ProductService();
		await productService.SaveBookToDatabaseAsync(new Book { Title = "Book", Author = "Author", Quantity = 5 });

		mockSqlCommand.Verify(x => x.ExecuteNonQueryAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}
