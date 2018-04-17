using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Models
{
    public class DTOCustomerOrder
    {
        public Customer Customer { get; set; }
        public Order Order { get; set; }
    }
}
