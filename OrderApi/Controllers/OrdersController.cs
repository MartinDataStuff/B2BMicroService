using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;
using OrderApi.Models;
using RestSharp;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IRepository<Order> repository;

        public OrdersController(IRepository<Order> repos)
        {
            repository = repos;
        }

        // GET: api/orders
        [HttpGet]
        [HttpGet("[action]")]
        public IEnumerable<Order> Get()
        {
            return repository.GetAll();
        }

        // GET api/orders/5
        [HttpGet("[action]/{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // Update api/orders/order
        [HttpPut("{order}")]
        public IActionResult Update(Order order)
        {
            repository.Edit(order);
            return new NoContentResult();
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

            List<int> listOfProductIds = new List<int>();
            foreach (var objectint in order.Items)
            {
                listOfProductIds.Add(objectint.ItemId);
            }

            foreach (var orderedProduct in response.Data)
            {
                //If item is not in order list, skip to next item.
                if (!listOfProductIds.Contains(orderedProduct.Id))
                {
                    continue;
                }

                int quantity = order.Items.FirstOrDefault(x => x.ItemId == orderedProduct.Id).NumberOfItem;

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
                else
                {
                    return NoContent();
                }

            }
            // If the order could  be created, route to get order.
            var newOrder = repository.Add(order);
            return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);
        }


        [HttpGet("[action]/{id}")]
        public IActionResult CalculateEstimatedDeliveryDate(int id)
        {
            Order order = repository.Get(id);
            if (order == null)
            {
                return NotFound();
            }
            DateTime dt = DateTime.Now;
            dt = dt.AddDays(3);

            return Json(dt);
        }

        [HttpGet("[action]/{CustomerReqNo}")]
        public IEnumerable<Order> GetAllFromCustomer(int CustomerReqNo)
        {
            return repository.GetAll().Where(order => order.CustomerId == CustomerReqNo).ToList();
        }

        // GET api/order/5
        [HttpGet("[action]/{orderNo}")]
        public IActionResult Cancel(int orderNo)
        {
            var order = repository.Get(orderNo);
            if (order == null)
            {
                return NotFound();
            }
            order.Staus = Order.OrderStaus.Cancelled;
            repository.Edit(order);
            return new ObjectResult(order);
        }
    }
}
