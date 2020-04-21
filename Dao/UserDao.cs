using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taskcore.Models;

namespace taskcore.Dao
{
    public class UserDao : Dao, IDao<User>
    {

        public static UserDao instance = null;
        public async Task<bool> Create(object obj)
        {
            User user = (User)obj;
            await getContext().AddAsync(user);
            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<User> Find(User model)
        {
            User user = await _context.User.FirstOrDefaultAsync(w => (w.Email == model.Email || w.Username == model.Email)
                        && w.Password == model.Password);
            return user;
        }

        public static UserDao getInstance()
        {
            return instance == null ? instance = new UserDao() : instance;
        }

        public Task<bool> Insert(object obj)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> Read(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Modify(object obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Erase(int id)
        {
            throw new NotImplementedException();
        }
    }
}