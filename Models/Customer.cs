using System;
using System.Collections.Generic;

namespace BizOrders_MVC.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? PrimeStartDate { get; set; }
        public DateTime? PrimeEndDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
