using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Gateway.Models
{
    public class APIGatewayData
    {

        public int Id { get; set; }
        public GatewayAPIS Gateways { get; set; }


        public enum GatewayAPIS { CustomerAPI, OrderAPI, ProductAPI }
    }
}
