using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taskcore.Manager;
using taskcore.Models;

namespace taskcore.Dao
{
    public class ProjectDao :  IDao<Project>
    {
        private static ProjectDao instance = null;
        private WorkDao wdao = null;
        private DatabaseContext _context = null;

        private ProjectDao() { }

        public async Task<bool> Accept(int id,int uid)
        {
            UserProjects usp = await GetContext().UserProjects
                .FirstOrDefaultAsync(w => w.UserId == uid && w.ProjectId == id);
            usp.IsAccept = true;
            GetContext().Update(usp);

            await GetContext().SaveChangesAsync();
            return true;
        }

        public async Task<bool> Decline(int id, int uid)
        {
            UserProjects usp = await GetContext().UserProjects
                .FirstOrDefaultAsync(w => w.UserId == uid && w.ProjectId == id);
            GetContext().Remove(usp);

            await GetContext().SaveChangesAsync();
            return true;
        }
        public  List<Project> Read(int id)
        {
            List<Project> result =  GetContext().Project.Where(w => GetContext().UserProjects
            .Where(e => e.UserId == id && e.IsAccept)
            .Select(c => c.ProjectId)
            .Contains(w.Id)).ToList();
            return result;
        }

        public async Task<bool> Insert(object obj)
        {
            Project project = (Project)obj;
            await GetContext().AddAsync(project);
            await GetContext().SaveChangesAsync();

            return true;
        }

        public async Task<bool> Insert(Project project,UserProjects usp)
        {
            await GetContext().AddAsync(project);
            await GetContext().SaveChangesAsync();
            usp.ProjectId = project.Id;
            await GetContext().AddAsync(usp);
            await GetContext().SaveChangesAsync();
            return true;
        }
        public async Task<bool> Modify(object obj)
        {
            Project tmp = (Project)obj;
            var project = GetContext().Project.First(w => w.Id == tmp.Id);
            GetContext().Entry(project).CurrentValues.SetValues(tmp);
            await GetContext().SaveChangesAsync();
            return true;
        }

        public async Task<bool> Erase(int id)
        {
            Project project = await GetContext().Project.FindAsync(id);
            GetContext().Remove(project);
            await GetContext().SaveChangesAsync();
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
            Project result = await GetContext().Project.FindAsync(itemid);
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
            await GetContext().SaveChangesAsync();
            return true;
        }

        public async Task<Project> Detail(int id)
        {
            return await GetContext().Project.FindAsync(id);
        }

        public async Task<bool> AddUserProject(Object obj)
        {
            UserProjects usp = (UserProjects)obj;
            await GetContext().AddAsync(usp);
            await GetContext().SaveChangesAsync();
            return true;
        }

        public async Task<List<ProjectsProgress>> ProgressList(int userId)
        {
            List<ProjectsProgress> result = new List<ProjectsProgress>();
            foreach (var item in  Read(userId))
            {
                ProjectsProgress tmp = new ProjectsProgress
                {
                    Project = item,
                    Progress = await GetWorkDao().ProgressPercentage(item.Id)
                };
                result.Add(tmp);
            }
            return result;
        }

        public WorkDao GetWorkDao()
        {
            if (wdao == null)
            {
                wdao = WorkDao.getInstance();
            }
            return wdao;
        }

        public async Task<bool> Request(int mateId, int projectId)
        {
            UserProjects usp = new UserProjects
            {
                UserId = mateId,
                ProjectId = projectId,
                IsAccept = false
            };
            await GetContext().AddAsync(usp);
            await GetContext().SaveChangesAsync();
            return await MailManager.ProjectRequestMessage(GetContext().User.Find(mateId), GetContext().Project.Find(projectId));
        }

        public async Task<List<Project>> RequestList(int id)
        {
            List<Project> projects = await GetContext().Project.Where(w => GetContext().UserProjects
                .Where(e => e.UserId == id && !e.IsAccept)
                .Select(c => c.ProjectId)
                .Contains(w.Id)).ToListAsync();

            return projects;
        }

        public DatabaseContext GetContext()
        {
            if(_context == null)
            {
                _context = DatabaseContext.getContext();
            }
            return _context;
        }
    }
}