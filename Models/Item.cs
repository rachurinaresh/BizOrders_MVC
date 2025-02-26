using System;
using System.Collections.Generic;

namespace BizOrders_MVC.Models
{
    public partial class Item
    {
        public int ItemId { get; set; }
        public int? OrderId { get; set; }
        public string? ItemName { get; set; }
        public double? ItemQuantity { get; set; }
        public double? ItemPrice { get; set; }
        public int? ItemDiscountEligible { get; set; }

        public virtual Order? Order { get; set; }
    }
}
