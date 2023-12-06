using Moq;
using Wholesaler.Services;

namespace Test
{
    public class ServiceWarTest
    {  //simple tests to check whether the interface works properly.

        private readonly Mock<IServiceWar> _serviceMock;

        public ServiceWarTest()
        {
            _serviceMock = new Mock<IServiceWar>();
        }

        [Fact]
        public async Task ExtractCsvInventory_ReturnsTrue()
        {
            // Arrange
            _serviceMock.Setup(x => x.ExtractCsvInventory()).ReturnsAsync(true);

            // Act
            var result = await _serviceMock.Object.ExtractCsvInventory();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExtractCsvPrices_ReturnsTrue()
        {
            // Arrange
            _serviceMock.Setup(x => x.ExtractCsvPrices()).ReturnsAsync(true);

            // Act
            var result = await _serviceMock.Object.ExtractCsvPrices();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExtractCsvProducts_ReturnsTrue()
        {
            // Arrange
            _serviceMock.Setup(x => x.ExtractCsvProducts()).ReturnsAsync(true);

            // Act
            var result = await _serviceMock.Object.ExtractCsvProducts();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetProductsBySKU_ReturnsObject()
        {
            // Arrange
            string sku = "test_sku";
            object expected = new object();
            _serviceMock.Setup(x => x.GetProductsBySKU(sku)).ReturnsAsync(expected);

            // Act
            var result = await _serviceMock.Object.GetProductsBySKU(sku);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}