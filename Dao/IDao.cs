using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tasky.Models;

namespace Tasky.Dao
{
    public interface IDao
    {
       public Task<Boolean> Create(Object obj);

    }
}