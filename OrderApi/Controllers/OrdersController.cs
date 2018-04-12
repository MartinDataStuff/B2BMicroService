using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;
using OrderApi.Models;
using RestSharp;

namespace OrderApi.Controllers
{
    [Route("api/Orders")]
    public class OrdersController : Controller
    {
        private readonly IRepository<Order> repository;

        public OrdersController(IRepository<Order> repos)
        {
            repository = repos;
        }

        // GET: api/orders
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return repository.GetAll();
        }

        // GET api/products/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/orders
        [HttpPost]
        public IActionResult Post([FromBody]Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            // Call ProductApi to get the product ordered
            RestClient c = new RestClient();
            // You may need to change the port number in the BaseUrl below
            // before you can run the request.
            c.BaseUrl = new Uri("http://productapi/api/products/");
            var request = new RestRequest(Method.GET);
            var response = c.Execute<List<Product>>(request);

            foreach (var orderedProduct in response.Data)
            {
                //If item is not in order list, skip to next item.
                if (!order.Items.ContainsKey(orderedProduct.Id))
                {
                    continue;
                }

                int quantity = -1;
                if (order.Items.TryGetValue(orderedProduct.Id, out quantity))
                {
                    if (quantity <= orderedProduct.ItemsInStock)
                    {
                        // reduce the number of items in stock for the ordered product,
                        // and create a new order.
                        orderedProduct.ItemsInStock -= quantity;
                        var updateRequest = new RestRequest(orderedProduct.Id.ToString(), Method.PUT);
                        updateRequest.AddJsonBody(orderedProduct);
                        var updateResponse = c.Execute(updateRequest);

                        if (!updateResponse.IsSuccessful)
                        {
                            // If the order could not be created, "return no content".
                            return NoContent();
                        }
                    }
                }
            }
            // If the order could  be created, route to get order.
            var newOrder = repository.Add(order);
            return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);


        }

    }
}
