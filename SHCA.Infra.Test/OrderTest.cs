using Moq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SHCA.Domain.Entities;
using SHCA.Infra.Data;
using SHCA.Infra.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SHCA.Infra.Test.Utilities;

namespace SHCA.Infra.Tests
{
    public class OrderRepositoryTests
    {
        private readonly Mock<ApiDbContext> _mockContext;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            var orders = new List<Order>
        {
            new Order { OrderId = 1, OrderDate = DateTime.Now },
            new Order { OrderId = 2, OrderDate = DateTime.Now }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Order>>();
            mockSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(orders.Provider);
            mockSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(orders.Expression);
            mockSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(orders.ElementType);
            mockSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(orders.GetEnumerator());
            mockSet.As<IAsyncEnumerable<Order>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<Order>(orders.GetEnumerator()));

            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids =>
            {
                var id = (long)ids[0];
                return new ValueTask<Order?>(orders.FirstOrDefault(o => o.OrderId == id));
            });

            _mockContext = new Mock<ApiDbContext>(new DbContextOptions<ApiDbContext>());
            _mockContext.Setup(c => c.Orders).Returns(mockSet.Object);

            _repository = new OrderRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsAllOrders()
        {
            // Act
            var result = await _repository.GetAllOrdersAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsOrder()
        {
            // Act
            var result = await _repository.GetOrderByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.OrderId);
        }

        [Fact]
        public async Task AddOrderAsync_AddsOrder()
        {
            // Arrange
            var order = new Order { OrderId = 134, OrderDate = DateTime.Now };
            var mockSet = new Mock<DbSet<Order>>();

            _mockContext.Setup(c => c.Orders).Returns(mockSet.Object);

            // Act
            await _repository.AddOrderAsync(order);

            // Assert
            mockSet.Verify(m => m.AddAsync(order, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_UpdatesOrder()
        {
            // Arrange
            var order = new Order { OrderId = 1, OrderDate = DateTime.Now };
            var mockSet = new Mock<DbSet<Order>>();

            _mockContext.Setup(c => c.Orders).Returns(mockSet.Object);

            // Act
            await _repository.UpdateOrderAsync(order);

            // Assert
            mockSet.Verify(m => m.Update(order), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_DeletesOrder()
        {
            // Arrange
            var order = new Order { OrderId = 1, OrderDate = DateTime.Now };
            var mockSet = new Mock<DbSet<Order>>();
            mockSet.Setup(m => m.FindAsync(It.IsAny<long>())).ReturnsAsync(order);

            _mockContext.Setup(c => c.Orders).Returns(mockSet.Object);

            // Act
            await _repository.DeleteOrderAsync(1);

            // Assert
            mockSet.Verify(m => m.Remove(order), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
