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
                new Order { Date = DateTime.Today, Items = CreateMockData(),CustomerId = 1 }
            };

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }

        static List<ProductQuantity> CreateMockData()
        {
            var products = new List<ProductQuantity>();
            int amountOfProducts = 5;
            for (int i = 0; i < amountOfProducts; i++)
            {
                products.Add(new ProductQuantity { ItemId = i, NumberOfItem = 10 });
            }
            return products;
        }
    }

}
