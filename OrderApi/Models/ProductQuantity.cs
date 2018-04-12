using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class ProductQuantity
    {
        public int Id { get; set; }
        public int NumberOfItem { get; set; }
        public int ItemId { get; set; }
    }
}
