using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taskcore.Models;

namespace taskcore.Dao
{
    public class UserDao : Dao
    {

        public static UserDao instance = null;
        public async Task<bool> Create(object obj)
        {
            User user = (User) obj;
            await getContext().AddAsync(user);
            await getContext().SaveChangesAsync();

            return true;
        }

        public static UserDao getInstance(){
            return instance == null ? instance = new UserDao() : instance;
        }
    }
}