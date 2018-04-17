﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public List<int> OrderIdList { get; set; } = new List<int>();
    }
}
