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

            // Look for any Customers
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            List<Customer> customers = new List<Customer>
            {
                new Customer { CompanyName = "Company1", Email = "example1@email.com", BillingAddress = "Home1" },
                new Customer { CompanyName = "Company2", Email = "example2@email.com", BillingAddress = "Home2" },
                new Customer { CompanyName = "Company3", Email = "example3@email.com", BillingAddress = "Home3" }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
