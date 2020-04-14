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
        private ProjectDao instance = null;
        

        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> UpdateStatus(int itemid, int statusid)
        {
            await getInstance().ModifyStatus(itemid, statusid);
            return Json(true);
        }

        public async Task<IActionResult> ReadToAll()
        {
            int? userId = HttpContext.Session.GetInt32("id");
            List<Project> result = await getInstance().Read((int)userId);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            int? userId = HttpContext.Session.GetInt32("id");

            await getInstance().Insert(project);
            await getInstance().AddUserProject(new UserProjects
            {
                ProjectId = project.Id,
                UserId = userId.Value,
                IsAccept = true
            });


            return Json(true);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await getInstance().Erase(id);
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Project project)
        {
            await getInstance().Modify(project);
            return Json(true);
        }
        public async Task<IActionResult> ProjectDetails(int id)
        {
            var project = await getInstance().Detail(id);
            return Json(project);
        }

        public ProjectDao getInstance()
        {
            if (instance == null)
            {
                instance = ProjectDao.getInstance();
            }
            return instance;
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
