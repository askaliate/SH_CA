using SHCA.Core.Entities;
using SHCA.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.Domain.Entities
{
    public class Order: Entity
    {
        public long ? OrderId { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? OrderDateUtc
        {
            get
            {
                return OrderDate.HasValue ? OrderDate.Value.ToUniversalTime() : (DateTime?)null;
            }
        }
        public IList<OrderItem>? Items { get; set; }

        //Customer Layer would be implemented as part of a separate feature set
        //public long CustomerId { get; set; }
        //public Customer Customer { get; set; }

        // Pricing processing module would be implemented as part of a separate feature set
        //public decimal? TotalPrice { get; set; }

        // Notification Preferences that can either be set at the customer level or order level would also be included
    }
}
