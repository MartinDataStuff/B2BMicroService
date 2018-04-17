using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerApi.Data;
using CustomerApi.Models;
using RestSharp;

namespace CustomerApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        private readonly IRepository<Customer> repository;

        public CustomersController(IRepository<Customer> repos)
        {
            repository = repos;
        }
        // GET: api/customers
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return repository.GetAll();
        }

        // GET api/customers/5
        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/customers
        [HttpPost]
        public IActionResult Post([FromBody]Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            var newCustomer = repository.Add(customer);

            return CreatedAtRoute("GetCustomer", new { id = newCustomer.Id }, newCustomer);
        }
        // Update api/customers/customer
        [HttpPut("{customer}")]
        public IActionResult Update(Customer customer)
        {
            repository.Edit(customer);
            return new NoContentResult();
        }
        // PUT api/customers/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Customer customer)
        {
            if (customer == null || customer.Id != id)
            {
                return BadRequest();
            }

            var modifiedCustomer = repository.Get(id);

            if (modifiedCustomer == null)
            {
                return NotFound();
            }

            modifiedCustomer.Phone = customer.Phone;
            modifiedCustomer.ShippingAddress = customer.ShippingAddress;
            modifiedCustomer.CompanyName = customer.CompanyName;
            modifiedCustomer.Email = customer.Email;
            modifiedCustomer.BillingAddress = customer.BillingAddress;

            repository.Edit(modifiedCustomer);
            return new NoContentResult();
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (repository.Get(id) == null)
            {
                return NotFound();
            }

            repository.Remove(id);
            return new NoContentResult();
        }

        [HttpGet("{id}")]//, Name = "GetCustomerCreditStanding")]
       // [Route("Customer/CreditStanding")]
        public IActionResult GetCreditStanding(int id)
        {
            if (repository.Get(id) == null)
            {
                return NotFound();
            }
            var customer = repository.Get(id);
            RestClient c = new RestClient();

            c.BaseUrl = new Uri("http://orderapi/api/order/");

            var request = new RestRequest(Method.GET);
            //Gets a list of all orders
            var response = c.Execute<List<Order>>(request);
            //Gets a list of all orders with this customer as owner
            var listOfAllOrdersFromCustomer = response.Data.Where(order => order.CustomerId == id);
            //Looks through the list to see if customer has any orders that hasn't been paid
            var hasNotPaid = listOfAllOrdersFromCustomer.FirstOrDefault(order => order.Staus == Order.OrderStaus.Requested) != null;
            return Json(hasNotPaid);
        }
    }
}