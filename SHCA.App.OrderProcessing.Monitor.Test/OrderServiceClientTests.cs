using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using Microsoft.Extensions.Logging;
using SHCA.App.OrderProcessing.Monitor.Auth;

namespace SHCA.App.OrderProcessing.Monitor.Process.Tests
{
    public class OrderServiceClientTests
    {
        [Fact]
        public async Task FetchMedicalEquipmentOrders_ShouldReturnOrdersData_WhenApiCallIsSuccessful()
        {
            // Arrange
            var mockHttpClient = new Mock<HttpClient>();
            var mockLogger = new Mock<ILogger<OrderServiceClient>>();
            var mockTokenHandler = new Mock<ITokenHandler>();

            mockTokenHandler.Setup(x => x.GetTokenAsync()).ReturnsAsync("fake-token");

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("orders-data")
                });

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new OrderServiceClient(httpClient, mockLogger.Object, mockTokenHandler.Object); // Cast to TokenHandler

            // Act
            var result = await service.FetchMedicalEquipmentOrders();

            // Assert
            Assert.Equal("orders-data", result);
        }
    }
}
