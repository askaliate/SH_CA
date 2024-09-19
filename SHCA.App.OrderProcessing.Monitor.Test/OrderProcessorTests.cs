using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SHCA.Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SHCA.App.OrderProcessing.Monitor.Auth;

namespace SHCA.App.OrderProcessing.Monitor.Process.Tests
{
    public class OrderProcessorTests
    {
        [Fact]
        public async Task ProcessOrdersData_ShouldProcessOrders_WhenOrdersDataIsValid()
        {
            // Arrange
            var testLogger = new TestLogger<OrderProcessor>();
            var mockHttpClient = new Mock<HttpClient>();
            var mockTokenHandler = new Mock<ITokenHandler>();

            // Setup the mock to return a fake token
            mockTokenHandler.Setup(t => t.GetTokenAsync()).ReturnsAsync("fake-token");

            var mockAlertServiceLogger = new TestLogger<AlertService>();
            var alertService = new AlertService(mockHttpClient.Object, mockAlertServiceLogger, mockTokenHandler.Object);

            var mockOrderUpdaterLogger = new TestLogger<OrderUpdateService>();
            var orderUpdater = new OrderUpdateService(mockHttpClient.Object, mockOrderUpdaterLogger, mockTokenHandler.Object);

            var processor = new OrderProcessor(testLogger, alertService, orderUpdater);

            var orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Status = "Delivered" }
                    }
                }
            };
            var validJson = JsonConvert.SerializeObject(orders);

            // Act
            await processor.ProcessOrdersData(validJson);

            // Assert
            Assert.DoesNotContain(testLogger.LogEntries, log => log.LogLevel == LogLevel.Error);
            Assert.Contains(testLogger.LogEntries, log => log.Message.Contains("Processing orders data..."));
            Assert.Contains(testLogger.LogEntries, log => log.Message.Contains("Orders data processed successfully."));
        }
    }
}
