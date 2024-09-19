using Moq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SHCA.Domain.Entities;
using SHCA.Infra.Data;
using SHCA.Infra.Repositories;
using SHCA.Infra.Test.Utilities;

namespace SHCA.Infra.Test
{
    public class OrderNotificationRepositoryTests
    {
        private readonly Mock<ApiDbContext> _mockContext;
        private readonly OrderNotificationRepository _repository;

        public OrderNotificationRepositoryTests()
        {
            var notifications = new List<OrderNotification>
        {
            new OrderNotification { NotificationId = 1, Message = "Order shipped" },
            new OrderNotification { NotificationId = 2, Message = "Order delivered" }
        }.AsAsyncQueryable();

            var mockSet = new Mock<DbSet<OrderNotification>>();
            mockSet.As<IQueryable<OrderNotification>>().Setup(m => m.Provider).Returns(notifications.Provider);
            mockSet.As<IQueryable<OrderNotification>>().Setup(m => m.Expression).Returns(notifications.Expression);
            mockSet.As<IQueryable<OrderNotification>>().Setup(m => m.ElementType).Returns(notifications.ElementType);
            mockSet.As<IQueryable<OrderNotification>>().Setup(m => m.GetEnumerator()).Returns(notifications.GetEnumerator());
            mockSet.As<IAsyncEnumerable<OrderNotification>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<OrderNotification>(notifications.GetEnumerator()));

            _mockContext = new Mock<ApiDbContext>(new DbContextOptions<ApiDbContext>());
            _mockContext.Setup(c => c.OrderNotifications).Returns(mockSet.Object);

            _repository = new OrderNotificationRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllNotifications_ReturnsAllNotifications()
        {
            // Act
            var result = await _repository.GetAllNotificationsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}