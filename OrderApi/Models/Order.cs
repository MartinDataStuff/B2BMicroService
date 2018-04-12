using System;
using System.Collections.Generic;

namespace OrderApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime? Date { get; set; }
        public OrderStaus Staus { get; set; }
        public decimal ShippedCharged { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }
        public List<ProductQuantity> Items { get; set; }


        public enum OrderStaus { Requested, Shipped, Completed, Cancelled, Paid }
    }
}
