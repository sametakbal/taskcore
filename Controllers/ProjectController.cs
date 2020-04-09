using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tasky.Models;

namespace Tasky.Controllers
{
    [UserFilter]
    public class ProjectController : Controller
    {

        private readonly DatabaseContext _context;
        public ProjectController(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> UpdateStatus(int itemid, int statusid)
        {
            var result = await _context.Project.FindAsync(itemid);
            result.Status = (Status)statusid;
            await _context.SaveChangesAsync();
            return Json(true);
        }
        public async Task<IActionResult> ReadToAll()
        {
            int? userId = HttpContext.Session.GetInt32("id");
            List<Project> result = await _context.Project.Where(w => _context.UserProjects
            .Where(e => e.UserId == userId && e.IsAccept)
            .Select(c => c.ProjectId)
            .Contains(w.Id)).ToListAsync();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            int? userId = HttpContext.Session.GetInt32("id");

            await _context.AddAsync(project);
            await _context.SaveChangesAsync();
            await _context.AddAsync(new UserProjects
            {
                ProjectId = project.Id,
                UserId = userId.Value,
                IsAccept = true
            });

            await _context.SaveChangesAsync();
            return Json(true);
        }
        public async Task<IActionResult> Delete(int id){
            var project = await _context.Project.FindAsync(id);
            _context.Remove(project);
            await _context.SaveChangesAsync();
            return Json(true);
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Update(Project project){
            _context.Update(project);
            await _context.SaveChangesAsync();
            return Json(true);
        }
        public async Task<IActionResult> ProjectDetails(int id)
        {
            var project = await _context.Project.FindAsync(id);
            return Json(project);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
