﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        private readonly IRepository<Product> repository;

        public ProductsController(IRepository<Product> repos)
        {
            repository = repos;
        }

        // GET: api/products
        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            return repository.GetAll();
        }

        // GET api/products/5
        [HttpGet("{id}", Name="GetProduct")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/products
        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            var newProduct = repository.Add(product);

            return CreatedAtRoute("GetProduct", new { id = newProduct.Id }, newProduct);
        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Product product)
        {
            if (product == null || product.Id != id)
            {
                return BadRequest();
            }

            var modifiedProduct = repository.Get(id);

            if (modifiedProduct == null)
            {
                return NotFound();
            }

            modifiedProduct.Name = product.Name;
            modifiedProduct.Price = product.Price;
            modifiedProduct.ItemsInStock = product.ItemsInStock;

            repository.Edit(modifiedProduct);
            return new NoContentResult();
        }

        // DELETE api/products/5
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
        // Update api/products/product
        [HttpPut("{product}")]
        public IActionResult Update(Product product)
        {
            repository.Edit(product);
            return new NoContentResult();
        }
        // GetValidCategories: api/string
        [HttpGet(Name = "GetValidCategories")]
        public IEnumerable<string> GetValidCategories()
        {
            List<string> returnVlaue = new List<string>();
            foreach (var item in repository.GetAll())
            {
                returnVlaue.Add(item.Description);
            }
            return returnVlaue.Distinct();
        }
    }
}
