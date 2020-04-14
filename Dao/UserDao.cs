using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tasky.Models;

namespace Tasky.Dao
{
    public class UserDao : Dao, IDao
    {
        public async Task<bool> Create(object obj)
        {
            User user = (User) obj;
            await getContext().AddAsync(user);
            await getContext().SaveChangesAsync();

            return true;
        }
    }
}