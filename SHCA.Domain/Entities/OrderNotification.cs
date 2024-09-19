using SHCA.Core.Entities ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.Domain.Entities
{
    public class OrderNotification : Entity
    {
        public long NotificationId { get; set; }
        DateTime NotificationDate { get; set; }
        Order? Order { get; set; } // Generic reference to the Order
        public string? Message { get; set; }
    }
}
