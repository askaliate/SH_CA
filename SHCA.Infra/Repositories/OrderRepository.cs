using Microsoft.EntityFrameworkCore;
using SHCA.Core.Interface;
using SHCA.Domain.Entities;
using SHCA.Domain.Interfaces.Repositories;
using SHCA.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.Infra.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApiDbContext _context;

        public OrderRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(long id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }
            return order;
        }

        public async Task AddOrderAsync(Order order)
        {
            if (order != null)
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }        
        }

        public async Task UpdateOrderAsync(Order order)
        {
            if (order != null)
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteOrderAsync(long id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

    }
}