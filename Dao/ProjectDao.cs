using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tasky.Models;

namespace Tasky.Dao
{
    public class ProjectDao : Dao,IDao
    {
        private static ProjectDao projectDao = null;

        private ProjectDao(){}
        public override async Task<List<Project>> Read(int id)
        {
            List<Project> result = await getContext().Project.Where(w => getContext().UserProjects
            .Where(e => e.UserId == id && e.IsAccept)
            .Select(c => c.ProjectId)
            .Contains(w.Id)).ToListAsync();
            return result;
        }

        public static ProjectDao GetProjectDao(){
            if(projectDao == null){
                projectDao = new ProjectDao();
            }
            return projectDao;
        }

        public bool metod()
        {
            throw new NotImplementedException();
        }
    }
}