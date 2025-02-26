using System;
using System.Collections.Generic;

namespace BizOrders_MVC.Models
{
    public partial class Order
    {
        public Order()
        {
            Items = new HashSet<Item>();
        }

        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? OrderPlacedTime { get; set; }
        public string? PackageName { get; set; }
        public long? DropAddressPinCode { get; set; }
        public int? NoOfItems { get; set; }
        public string? PaymentMode { get; set; }
        public double? Invoice { get; set; }
        public double? GrandTotal { get; set; }
        public double? PayableAmount { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
