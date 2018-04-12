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
        public Dictionary<int, int> Items { get; set; } //key = item id, value = quantity, keys also act as list of items.


        public enum OrderStaus { Requested, Shipped, Completed, Cancelled, Paid }
    }
}
