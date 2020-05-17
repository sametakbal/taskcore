using System.Collections.Generic;
using System.Threading.Tasks;
using taskcore.Models;

namespace taskcore.Dao
{
    public class DaOperations
    {
        public DatabaseContext _context = null;

        private static DaOperations instance = null;
        private static readonly object padlock = new object();
        

        private ProjectDao projectDao = null;
        private WorkDao workDao = null;


        public async Task<bool> ProjectInsert(Project proj,UserProjects userProjects)
        {
            return await GetProjectDao().Insert(proj,userProjects);
        }

        public async Task<bool> ProjectAccept(int id,int uid)
        {
            return await GetProjectDao().Accept(id, uid);
        }
        public async Task<bool> ProjectDecline(int id, int uid)
        {
            return await GetProjectDao().Decline(id, uid);
        }

        public List<Project> ProjectRead(int id)
        {
            return GetProjectDao().Read(id);
        }

        public async Task<bool> ProjectModify(Project proj)
        {
            return await GetProjectDao().Modify(proj);
        }

        public async Task<List<ProjectsProgress>> ProjectProgressList(int id)
        {
            return await GetProjectDao().ProgressList(id);
        }

        public async Task<bool> ProjectErase(int id)
        {
            return await GetProjectDao().Erase(id);
        }

        public async Task<bool> ProjectModifyStatus(int itemid, int statusid)
        {
            return await GetProjectDao().ModifyStatus(itemid, statusid);
        }

        public async Task<Project> ProjectDetail(int id)
        {
            return await GetProjectDao().Detail(id);
        }

        public async Task<bool> ProjectRequest(int mateId, int projectId)
        {
            return await GetProjectDao().Request(mateId,projectId);
        }


        public async Task<List<Project>> ProjectRequestList(int id)
        {
            return await GetProjectDao().RequestList(id);
        }
        public DatabaseContext getContext()
        {
            if (_context == null)
            {
                _context = DatabaseContext.getContext();
            }
            return _context;
        }
        public static UserDao GetUserDao()
        {
            return UserDao.getInstance();
        }
        public ProjectDao GetProjectDao()
        {
            return projectDao == null ? projectDao = ProjectDao.getInstance() : projectDao;
        }
        public WorkDao GetWorkDao()
        {
            return workDao == null ? workDao = WorkDao.getInstance() : workDao;
        }

        public static DaOperations GetInstance()
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DaOperations();
                    }
                }
            }
            return instance;
        }
    }
}