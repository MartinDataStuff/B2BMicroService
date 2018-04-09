using System;
namespace OrderApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public OrderStaus Staus { get; set; }
        public decimal ShippedCharged { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }


        public enum OrderStaus { Requested, Shipped, Completed, Cancelled, Paid }
    }
}
