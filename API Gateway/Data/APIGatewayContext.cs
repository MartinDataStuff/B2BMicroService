using Microsoft.EntityFrameworkCore;
using API_Gateway.Models;

namespace API_Gateway.Data
{
    public class APIGatewayContext : DbContext
    {
        public APIGatewayContext(DbContextOptions<APIGatewayContext> options) 
            : base(options)
        {
            
        }

        public DbSet<APIGatewayData> GatewayDB { get; }
    }
}
