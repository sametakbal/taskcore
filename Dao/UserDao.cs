using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using taskcore.Manager;
using taskcore.Models;
using taskcore.Observer;

namespace taskcore.Dao
{
    public class UserDao : DaOperations, IDao<User>
    {

        public static UserDao instance = null;
        public User user = null;
        public UserObserver observer = null;
        public async Task<bool> Create(object obj)
        {
            User user = (User)obj;
            await getContext().AddAsync(user);
            await getContext().SaveChangesAsync();
            getObserver().Create(user);
            return true;
        }

        public async Task<User> Find(User model)
        {
            User user = await getContext().User.FirstOrDefaultAsync(w => (w.Email == model.Email || w.Username == model.Email)
                        && w.Password == model.Password);
            return user;
        }

        public List<User> FriendList(int id)
        {
            List<User> flist = new List<User>();
            List<UserMates> userMates = getContext().UserMates.Where(w => w.UserId == id).ToList();
            foreach (var item in userMates)
            {
                var tmp = getContext().User.FirstOrDefault(w => w.Id == item.MateId);
                flist.Add(tmp);
            }
            return flist;
        }

        public static UserDao getInstance()
        {
            return instance == null ? instance = new UserDao() : instance;
        }

        public async Task<bool> Insert(object obj)
        {
            User user = (User)obj;
            await getContext().AddAsync(user);
            await getContext().SaveChangesAsync();
            return true;
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
            getObserver().Update(tmp);
            return true;
        }

        public async Task<bool> Erase(int id)
        {
            var user = await getContext().User.FindAsync(id);
            getContext().Remove(user);
            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFriend(int uid,int mid)
        {
            UserMates usm1 = await getContext().UserMates.FirstOrDefaultAsync(w=> w.UserId == uid && w.MateId==mid);
            UserMates usm2 = await getContext().UserMates.FirstOrDefaultAsync(w=> w.UserId == mid && w.MateId==uid);

            getContext().Remove(usm1);
            getContext().Remove(usm2);
            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUser(int id)
        {
            return await getContext().User.FindAsync(id);
        }

        public async Task<User> Find(string email)
        {
            return await getContext().User.FirstOrDefaultAsync(w => w.Email == email);
        }

        public async Task<bool> InserCode(PasswordCode passwordCode)
        {
            await getContext().AddAsync(passwordCode);
            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<PasswordCode>  getPasswordCode(string code)
        {
             return await getContext().PasswordCode.FirstOrDefaultAsync(w => w.Code.Equals(code));
        }

        public async Task<bool> RemoveCode(PasswordCode psd)
        {
            getContext().Remove(psd);
            await getContext().SaveChangesAsync();
            return true;
        }

        public DatabaseContext GetContext()
        {
            return DatabaseContext.getContext();
        }

        public UserObserver getObserver()
        {
            if(observer == null)
            {
                observer = new UserObserver();
            }
            return observer;
        }
    }
}