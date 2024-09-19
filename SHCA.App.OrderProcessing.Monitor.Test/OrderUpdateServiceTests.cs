using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using Microsoft.Extensions.Logging;
using SHCA.App.OrderProcessing.Monitor.Process;
using SHCA.Domain.Entities;
using System.Collections.Generic;
using SHCA.App.OrderProcessing.Monitor.Auth;

namespace SHCA.App.OrderProcessing.Monitor.Process.Tests
{
    public class OrderUpdateServiceTests
    {
        [Fact]
        public async Task SendAlertAndUpdateOrder_ShouldLogInformation_WhenApiCallIsSuccessful()
        {
            // Arrange
            var testLogger = new TestLogger<OrderUpdateService>();
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

            var service = new OrderUpdateService(httpClient, testLogger, mockTokenHandler.Object);

            var order = new Order
            {
                OrderId = 1,
                Items = new List<OrderItem>
                {
                    new OrderItem { Status = "Delivered" }
                }
            };

            // Act
            await service.SendAlertAndUpdateOrder(order);

            // Assert
            Assert.Contains(testLogger.LogEntries, log => log.Message.Contains("Order 1 updated successfully."));
        }
    }
}
