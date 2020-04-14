using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tasky.Dao;
using Tasky.Models;

namespace Tasky.Controllers
{
    [UserFilter]
    public class ProjectController : Controller
    {

        private DatabaseContext _context = null;
        private ProjectDao _pdao = null;
        public ProjectController()
        {
        }

        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> UpdateStatus(int itemid, int statusid)
        {
            var result = await getContext().Project.FindAsync(itemid);
            result.Status = (Status)statusid;
            await getContext().SaveChangesAsync();
            return Json(true);
        }

        public async Task<IActionResult> ReadToAll()
        {
            int? userId = HttpContext.Session.GetInt32("id");
            List<Project> result = await getPdao().Read((int)userId);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            int? userId = HttpContext.Session.GetInt32("id");

            await getPdao().Create(project);
            await getContext().AddAsync(new UserProjects
            {
                ProjectId = project.Id,
                UserId = userId.Value,
                IsAccept = true
            });

            await getContext().SaveChangesAsync();
            return Json(true);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var project = await getContext().Project.FindAsync(id);
            getContext().Remove(project);
            await getContext().SaveChangesAsync();
            return project != null ? Json(true) : Json(false);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Project project)
        {
            getContext().Update(project);
            await getContext().SaveChangesAsync();
            return Json(true);
        }
        public async Task<IActionResult> ProjectDetails(int id)
        {
            var project = await getContext().Project.FindAsync(id);
            return Json(project);
        }

        public DatabaseContext getContext()
        {
            if (_context == null)
            {
                _context = DatabaseContext.getContext();
            }
            return _context;
        }

        public ProjectDao getPdao()
        {
            if (_pdao == null)
            {
                _pdao = ProjectDao.GetProjectDao();
            }
            return _pdao;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
