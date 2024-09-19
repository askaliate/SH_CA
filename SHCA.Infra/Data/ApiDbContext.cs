using Microsoft.EntityFrameworkCore;
using SHCA.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.Infra.Data
{
    public class ApiDbContext : DbContext
    {
        public virtual DbSet<OrderNotification> OrderNotifications { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }
    }
}
