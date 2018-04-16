using System;
using System.Collections.Generic;

namespace API_Gateway.Data
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(string id);


    }
}
