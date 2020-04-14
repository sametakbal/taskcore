using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tasky.Models;

namespace Tasky.Dao
{
    public class WorkDao : Dao, IDao<Work>
    {
        private static WorkDao instance = null;

        private WorkDao() { }


        public static WorkDao getInstance()
        {
            if (instance == null)
            {
                instance = new WorkDao();
            }
            return instance;
        }

        public async Task<bool> Erase(int id)
        {
            Work work = await getContext().Work.FindAsync(id);
            getContext().Remove(work);
            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insert(object obj)
        {
            Work work = (Work)obj;
            await getContext().AddAsync(work);
            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<bool> Modify(object obj)
        {
            Work work = (Work)obj;
            getContext().Update(work);
            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<List<Work>> Read(int userId, int ProjectId)
        {
            List<Work> result = await getContext().Work.Where(w => getContext().UserWorks
            .Where(e => e.UserId == userId)
                .Select(c => c.WorkId)
                .Contains(w.Id) && w.ProjectId == ProjectId).ToListAsync();
            return result;
        }

        public async Task<bool> ModifyStatus(int itemid, int statusid)
        {
            var result = await getContext().Work.FindAsync(itemid);
            result.Status = (Status)statusid;
            switch (result.Status)
            {
                case Status.NotStarted:
                    result.StartTime = DateTime.Now;
                    result.FinishTime = null;
                    break;
                case Status.Done:
                    result.FinishTime = DateTime.Now;
                    break;
                default:
                    result.FinishTime = null;
                    result.StartTime = null;
                    break;
            }
            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<int> ProgressPercentage(int id)
        {
            List<Work> finishedTasks = await getContext().Work.Where(w => w.FinishTime != null && w.ProjectId == id).ToListAsync();
            List<Work> tasks = await getContext().Work.Where(w => w.ProjectId == id).ToListAsync();
            var result = (decimal)((decimal)finishedTasks.Count() / (decimal)tasks.Count()) * 100;
            return (int)result;
        }
        public async Task<bool> InsertUserWork(UserWorks userWorks)
        {
            await getContext().AddAsync(userWorks);
            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<Work> Detail(int id){
            Work work = await getContext().Work.FindAsync(id);
            return work;
        }
        public async Task<bool> ProjectCheck(int id){
            Project pr = await getContext().Project.FindAsync(id);
            return pr != null ? true : false;
        }

        
        public Task<List<Work>> Read(int id)
        {
            throw new NotImplementedException();
        }
    }
}