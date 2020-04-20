using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using taskcore.Models;

namespace taskcore.Dao
{
    public interface IDao<T>
    {
       public Task<Boolean> Insert(Object obj);

       public Task<List<T>> Read(int id);
       public Task<Boolean> Modify(Object obj);
       public Task<Boolean> Erase(int id);

    }
}