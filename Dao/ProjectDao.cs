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
    public class ProjectDao : Dao, IDao<Project>
    {
        private static ProjectDao instance = null;
        private WorkDao wdao=null;

        private ProjectDao() { }
        
        public async Task<bool> Accept(int id)
        {
            UserProjects usp = await getContext().UserProjects
                .FirstOrDefaultAsync(w => w.UserId == UserManager.GetCurrentUser().Id && w.ProjectId == id);
            usp.IsAccept = true;
            getContext().Update(usp);

            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<bool> Decline(int id)
        {
            UserProjects usp = await getContext().UserProjects
                .FirstOrDefaultAsync(w => w.UserId == UserManager.GetCurrentUser().Id && w.ProjectId == id);
            getContext().Remove(usp);

            await getContext().SaveChangesAsync();
            return true;
        }
        public async Task<List<Project>> Read(int id)
        {
            List<Project> result = await getContext().Project.Where(w => getContext().UserProjects
            .Where(e => e.UserId == id && e.IsAccept)
            .Select(c => c.ProjectId)
            .Contains(w.Id)).ToListAsync();
            return result;
        }

        public async Task<bool> Insert(object obj)
        {
            Project project = (Project)obj; 
            await getContext().AddAsync(project);
            await getContext().SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> Modify(object obj)
        {
            Project tmp = (Project)obj;
            var project = getContext().Project.First(w => w.Id == tmp.Id);
            getContext().Entry(project).CurrentValues.SetValues(tmp);
            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<bool> Erase(int id)
        {
            Project project = await getContext().Project.FindAsync(id);
            getContext().Remove(project);
            await getContext().SaveChangesAsync();
            return true;
        }

        public static ProjectDao getInstance()
        {
            if (instance == null)
            {
                instance = new ProjectDao();
            }
            return instance;
        }

        public async Task<bool> ModifyStatus(int itemid, int statusid)
        {
            Project result = await getContext().Project.FindAsync(itemid);
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

        public async Task<Project> Detail(int id){
           return await getContext().Project.FindAsync(id);
        }

        public async Task<bool> AddUserProject(Object obj){
            UserProjects usp = (UserProjects) obj;
            await getContext().AddAsync(usp);
            await getContext().SaveChangesAsync();            
            return true;
        }

        public async Task<List<ProjectsProgress>> ProgressList(int userId){
            List<ProjectsProgress> result = new List<ProjectsProgress>();
            foreach(var item in await Read(userId)){
                ProjectsProgress tmp = new ProjectsProgress{
                    Project = item,
                    Progress = await GetWorkDao().ProgressPercentage(item.Id)
                };
                result.Add(tmp);
            }
            return result;
        }

        public WorkDao GetWorkDao(){
            if(wdao == null){
                wdao = WorkDao.getInstance();
            }
            return wdao;
        }

        public async Task<bool> Request(int mateId,int projectId)
        {
            UserProjects usp = new UserProjects
            {
                UserId = mateId,
                ProjectId = projectId,
                IsAccept = false
            };
            await getContext().AddAsync(usp);
            await getContext().SaveChangesAsync();
            return true;
        }

        public async Task<List<Project>> RequestList()
        {
            List<Project> projects = await getContext().Project.Where(w => getContext().UserProjects
                .Where(e => e.UserId == UserManager.GetCurrentUser().Id && !e.IsAccept)
                .Select(c => c.ProjectId)
                .Contains(w.Id)).ToListAsync();

            return projects;
        }
    }
}