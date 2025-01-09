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
		var mockSqlCommand = new Mock<SqlCommand>();
		mockSqlCommand.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

		var mockProductService = new Mock<ProductService> { CallBase = true };
	}
}