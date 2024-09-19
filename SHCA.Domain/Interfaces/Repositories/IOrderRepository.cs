using SHCA.Core.Interface;
using SHCA.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
    }
}
