using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Gateway.Models;
using API_Gateway.Data;

namespace API_Gateway.Data
{
    public class APIGatewayRepository : IRepository<APIGatewayData>
    {
        private readonly APIGatewayContext db;
        
        public APIGatewayRepository(APIGatewayContext context)
        {
            db = context;
        }

        public APIGatewayData Get(string id)
        {
            return db.GatewayDB.FirstOrDefault(o => o.Gateways.ToString() == id);
        }

        public IEnumerable<APIGatewayData> GetAll()
        {
            return db.GatewayDB.ToArray();
        }
    }
}
