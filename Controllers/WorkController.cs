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
    public class WorkController : Controller
    {
        private readonly DatabaseContext _context;
        public WorkController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ProgressPercentage(int id)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            List<Work> finishedTasks = await _context.Work.Where(w => w.FinishTime != null && w.ProjectId == id).ToListAsync();
            List<Work> tasks = await _context.Work.Where(w => w.ProjectId == id).ToListAsync();
            int result =(int) (decimal)((decimal)finishedTasks.Count() / (decimal)tasks.Count()) * 100;
            return Json(result);
        }
        public async Task<IActionResult> Index(int id)
        {

            int? userId = HttpContext.Session.GetInt32("id");
            var result = await _context.Project.FirstOrDefaultAsync(w => w.Id == id);
            if (result == null)
            {
                return Redirect("/Project/Index");
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Work.FindAsync(id);
            _context.Remove(task);
            await _context.SaveChangesAsync();
            return Json(true);
        }
        public async Task<IActionResult> Save(Work work)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            if (work.Id == 0)
            {
                _context.Add(work);
                await _context.SaveChangesAsync();
                await _context.UserWorks.AddAsync(new UserWorks { WorkId = work.Id, UserId = userId.Value });
            }
            else
            {
                _context.Update(work);
            }
            await _context.SaveChangesAsync();
            return Json(true);
        }
        public async Task<IActionResult> List(int Id)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            //   var result = await _context.Task.Where(w => w.ProjectId == Id && w.UserId == userId).ToListAsync();
            List<Work> result = await _context.Work.Where(w => _context.UserWorks
            .Where(e => e.UserId == userId)
                .Select(c => c.WorkId)
                .Contains(w.Id) && w.ProjectId == Id).ToListAsync();
            return Json(result);
        }
        public async Task<IActionResult> UpdateStatus(int itemid, int statusid)
        {
            var result = await _context.Work.FindAsync(itemid);
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
            await _context.SaveChangesAsync();
            return Json(true);
        }
        public async Task<IActionResult> WorkDetails(int id)
        {
            var work = await _context.Work.FindAsync(id);
            return Json(work);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
