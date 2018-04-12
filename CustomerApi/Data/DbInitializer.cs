using CustomerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data
{
    public class DbInitializer
    {
        // This method will create and seed the database.
        public static void Initialize(CustomerApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Products
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            List<Customer> customers = new List<Customer>
            {
                new Customer { CompanyName = "CompanyOne", Email = "example@email.com", BillingAddress = "Home" }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
