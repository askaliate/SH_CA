using SHCA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.Domain.Entities
{
    public class OrderItem : Entity 
    {
        long OrderItemId { get; set; }
        // Order Items Layer would be implemented as part of a separate feature set
        //string? ProductName { get; set; }
        //int Quantity { get; set; }
        //decimal Price { get; set; }
        public string? Status { get; set; }
        Order? Order { get; set; } // Reference back to the Order
        public object? Description { get; set; }
        public int? DeliveryNotificationCount { get; set; }
    }
}
