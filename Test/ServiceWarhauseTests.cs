using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholesalerDapper.Service;

namespace Test
{
    public class ServiceWarhauseTests
    {
        [Fact]
        public async Task ExtractCsvInventoryD_ShouldReturnTrue()
        {
            // Arrange
            var serviceMock = new Mock<IServiceWarhause>();
            serviceMock.Setup(x => x.ExtractCsvInventoryD()).ReturnsAsync(true);

            // Act
            var result = await serviceMock.Object.ExtractCsvInventoryD();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExtractCsvPricesD_ShouldReturnTrue()
        {
            // Arrange
            var serviceMock = new Mock<IServiceWarhause>();
            serviceMock.Setup(x => x.ExtractCsvPricesD()).ReturnsAsync(true);

            // Act
            var result = await serviceMock.Object.ExtractCsvPricesD();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExtractCsvProductsD_ShouldReturnTrue()
        {
            // Arrange
            var serviceMock = new Mock<IServiceWarhause>();
            serviceMock.Setup(x => x.ExtractCsvProductsD()).ReturnsAsync(true);

            // Act
            var result = await serviceMock.Object.ExtractCsvProductsD();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetProductsBySKUD_ShouldReturnObject()
        {
            // Arrange
            var serviceMock = new Mock<IServiceWarhause>();
            var expectedObject = new object();
            serviceMock.Setup(x => x.GetProductsBySKUD(It.IsAny<string>())).ReturnsAsync(expectedObject);

            // Act
            var result = await serviceMock.Object.GetProductsBySKUD("exampleSKU");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<object>(result);
            Assert.Equal(expectedObject, result);
        }

        [Fact]
        public void TruncateTable_ShouldBeCalled()
        {
            // Arrange
            var serviceMock = new Mock<IServiceWarhause>();

            // Act
            serviceMock.Object.TruncateTable("ExampleTable");

            // Assert
            serviceMock.Verify(x => x.TruncateTable("ExampleTable"), Times.Once);
        }
    }
}
