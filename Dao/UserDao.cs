using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taskcore.Manager;
using taskcore.Models;

namespace taskcore.Dao
{
    public class UserDao : Dao, IDao<User>
    {

        public static UserDao instance = null;
        public User user = null;
        public async Task<bool> Create(object obj)
        {
            User user = (User)obj;
            await getContext().AddAsync(user);
            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<User> Find(User model)
        {
            User user = await getContext().User.FirstOrDefaultAsync(w => (w.Email == model.Email || w.Username == model.Email)
                        && w.Password == model.Password);
            return user;
        }

        public List<User> FriendList()
        {
            List<User> flist = new List<User>();
            List<UserMates> userMates = getContext().UserMates.Where(w=> w.UserId == GetUser().Id).ToList();
            foreach(var item in userMates){
                var tmp = getContext().User.FirstOrDefault(w=>w.Id == item.MateId) ;
                flist.Add(tmp);
            }
            return flist;
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

        public async Task<bool> Modify(object obj)
        {
            User tmp = (User)obj;
            var user = getContext().User.First(w => w.Id == tmp.Id);
            getContext().Entry(user).CurrentValues.SetValues(tmp);
            await getContext().SaveChangesAsync();
            return true;
        }

        public Task<bool> Erase(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUser()
        {
            return user == null ? user = UserManager.GetCurrentUser() : user;
        }
    }
}