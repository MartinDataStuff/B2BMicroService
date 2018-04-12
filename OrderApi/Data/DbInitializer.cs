using System.Collections.Generic;
using System.Linq;
using OrderApi.Models;
using System;

namespace OrderApi.Data
{
    public static class DbInitializer
    {
        // This method will create and seed the database.
        public static void Initialize(OrderApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Products
            if (context.Orders.Any())
            {
                return;   // DB has been seeded
            }



            List<Order> orders = new List<Order>
            {
                new Order { Date = DateTime.Today, Items = CreateMockData()}
            };

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }

        static Dictionary<int, int> CreateMockData() {
            var products = new Dictionary<int, int>();
            int amountOfProducts = 5;
            for (int i = 0; i < amountOfProducts; i++)
            {
                products.Add(i, 10);
            }
            return products;
        }
    }
}
