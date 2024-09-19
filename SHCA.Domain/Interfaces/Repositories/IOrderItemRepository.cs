using SHCA.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.Domain.Interfaces.Repositories
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync();
    }
}
