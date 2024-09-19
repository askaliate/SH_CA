using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using SHCA.Domain.Entities;
using SHCA.App.OrderProcessing.Monitor.Auth;

namespace SHCA.App.OrderProcessing.Monitor.Process.Tests
{
    public class AlertServiceTests
    {
        [Fact]
        public async Task SendAlertMessage_ShouldLogInformation_WhenApiCallIsSuccessful()
        {
            // Arrange
            var testLogger = new TestLogger<AlertService>();
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
                    StatusCode = HttpStatusCode.OK
                });

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new AlertService(httpClient, testLogger, mockTokenHandler.Object);

            var orderItem = new OrderItem { Description = "Item1", Status = "Delivered" };

            // Act
            await service.SendAlertMessage(orderItem, "123");

            // Assert
            Assert.Contains(testLogger.LogEntries, log => log.Message.Contains("Alert for delivered item: Order 123, Item: Item1, Delivery Notifications: Delivered"));
        }
    }
}
