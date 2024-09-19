using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using SHCA.App.OrderProcessing.Monitor.Process;
using SHCA.Domain.Entities;
using System.Net.Http;
using SHCA.App.OrderProcessing.Monitor.Interfaces;
using SHCA.App.OrderProcessing.Monitor.Auth;

namespace SHCA.App.OrderProcessing.Monitor.Process.Tests
{
    public class OrderStatusProcessorServiceTests
    {
        [Fact]
        public async Task ProcessOrders_ShouldCallProcessOrdersData_WhenOrdersDataIsNotNull()
        {
            // Arrange
            var mockOrderFetcher = new Mock<IOrderServiceClient>();
            var mockLogger = new Mock<ILogger<OrderStatusProcessorService>>();

            var mockHttpClient = new Mock<HttpClient>();
            var mockTokenHandler = new Mock<ITokenHandler>();
            var mockAlertServiceLogger = new Mock<ILogger<AlertService>>();
            var mockOrderUpdaterLogger = new Mock<ILogger<OrderUpdateService>>();

            var alertService = new AlertService(mockHttpClient.Object, mockAlertServiceLogger.Object, mockTokenHandler.Object);
            var orderUpdater = new OrderUpdateService(mockHttpClient.Object, mockOrderUpdaterLogger.Object, mockTokenHandler.Object);

            var mockOrderProcessor = new Mock<IOrderProcessor>();

            var ordersData = "[{\"OrderId\":1,\"Items\":[{\"Status\":\"Delivered\"}]}]";
            mockOrderFetcher.Setup(x => x.FetchMedicalEquipmentOrders()).ReturnsAsync(ordersData);
            mockOrderProcessor.Setup(x => x.ProcessOrdersData(ordersData)).Returns(Task.CompletedTask);

            var service = new OrderStatusProcessorService(mockOrderFetcher.Object, mockOrderProcessor.Object, mockLogger.Object);

            // Act
            await service.ProcessOrders();

            // Assert
            mockOrderFetcher.Verify(x => x.FetchMedicalEquipmentOrders(), Times.Once);
            mockOrderProcessor.Verify(x => x.ProcessOrdersData(ordersData), Times.Once);
        }

        [Fact]
        public async Task ProcessOrders_ShouldLogError_WhenOrdersDataIsNull()
        {
            // Arrange
            var mockOrderFetcher = new Mock<IOrderServiceClient>();
            var mockLogger = new TestLogger<OrderStatusProcessorService>();

            var mockHttpClient = new Mock<HttpClient>();
            var mockTokenHandler = new Mock<ITokenHandler>();
            var mockAlertServiceLogger = new Mock<ILogger<AlertService>>();
            var mockOrderUpdaterLogger = new Mock<ILogger<OrderUpdateService>>();

            var alertService = new AlertService(mockHttpClient.Object, mockAlertServiceLogger.Object, mockTokenHandler.Object);
            var orderUpdater = new OrderUpdateService(mockHttpClient.Object, mockOrderUpdaterLogger.Object, mockTokenHandler.Object);

            var mockOrderProcessor = new Mock<IOrderProcessor>();

            mockOrderFetcher.Setup(x => x.FetchMedicalEquipmentOrders()).ReturnsAsync((string?)null);

            var service = new OrderStatusProcessorService(mockOrderFetcher.Object, mockOrderProcessor.Object, mockLogger);

            // Act
            await service.ProcessOrders();

            // Assert
            mockOrderFetcher.Verify(x => x.FetchMedicalEquipmentOrders(), Times.Once);
            mockOrderProcessor.Verify(x => x.ProcessOrdersData(It.IsAny<string>()), Times.Never);
            Assert.Contains(mockLogger.LogEntries, log => log.LogLevel == LogLevel.Error && log.Message.Contains("No orders data to process."));
        }
    }
}
