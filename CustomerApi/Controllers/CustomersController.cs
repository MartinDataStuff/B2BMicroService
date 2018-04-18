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
    [Route("api/Customers/")]
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
        [HttpGet("[action]/{id}", Name = "GetCustomer")]
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

        [HttpGet("[action]/{id}")]
        public IActionResult GetCreditStanding(int id)
        {
            if (repository.Get(id) == null)
            {
                return NotFound();
            }
            var customer = repository.Get(id);

            RestClient c = new RestClient();
            c.BaseUrl = new Uri("http://orderapi/api/orders/GetAllFromCustomer/");

            var request = new RestRequest(id.ToString(), Method.GET);
            //Gets a list of all orders with this customer as owner
            var response = c.Execute<List<Order>>(request);
            //Looks through the list to see if customer has any orders that hasn't been paid
            var hasNotPaid = response.Data.FirstOrDefault(order => order.Staus == Order.OrderStaus.Requested) != null;
            return Json(hasNotPaid);
        }

        [HttpPost("[action]")]
        public IActionResult PlaceOrder([FromBody]DTOCustomerOrder customerOrder)
        {
            var customer = repository.Get(customerOrder.Customer.Id);
            IActionResult answer = Content("No new customer were needed");
            if (customer == null)
            {
                customer = repository.Add(customerOrder.Customer);
                answer = CreatedAtRoute("GetCustomer", new { id = customer.Id }, customer);
            }
            customerOrder.Order.CustomerId = customer.Id;

            RestClient c = new RestClient();
            c.BaseUrl = new Uri("http://orderapi/api/order/");
            var request = new RestRequest(Method.POST);
            request.AddBody(customerOrder.Order);
            var response = c.Execute<Order>(request);
            return answer;
        }
    }
}