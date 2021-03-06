﻿using CustomerApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly CustomerApiContext db;

        public CustomerRepository(CustomerApiContext context)
        {
            db = context;
        }
        public Customer Add(Customer entity)
        {
            var newCustomer = db.Customers.Add(entity).Entity;
            db.SaveChanges();
            return newCustomer;
        }

        public void Edit(Customer entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Customer Get(int id)
        {
            return db.Customers.FirstOrDefault(cust => cust.Id == id);
        }

        public IEnumerable<Customer> GetAll()
        {

            var list = db.Customers.ToList();
            return db.Customers.ToList();
        }

        public void Remove(int id)
        {
            var customer = db.Customers.FirstOrDefault(cust => cust.Id == id);
            db.Customers.Remove(customer);
            db.SaveChanges();
        }
    }
}
