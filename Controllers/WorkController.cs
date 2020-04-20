using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using taskcore.Dao;
using taskcore.Models;

namespace taskcore.Controllers
{
    [UserFilter]
    public class WorkController : Controller
    {
        private WorkDao instance = null;
        public async Task<IActionResult> ProgressPercentage(int id)
        {
            return Json(await getInstance().ProgressPercentage(id));
        }
        public async Task<IActionResult> Index(int id)
        {
            if (!await getInstance().ProjectCheck(id))
            {
                return Redirect("/Project/Index");
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            return Json(await getInstance().Erase(id));
        }
        public async Task<IActionResult> Save(Work work)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            if (work.Id == 0)
            {
                await getInstance().Insert(work);
                await getInstance().InsertUserWork(new UserWorks { WorkId = work.Id, UserId = userId.Value });
            }
            else
            {
                await getInstance().Modify(work);
            }
            return Json(true);
        }
        public async Task<IActionResult> List(int Id)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            //   var result = await getContext().Task.Where(w => w.ProjectId == Id && w.UserId == userId).ToListAsync();
            List<Work> result = await getInstance().Read((int)userId, Id);
            return Json(result);
        }
        public async Task<IActionResult> UpdateStatus(int itemid, int statusid)
        {
            await getInstance().ModifyStatus(itemid, statusid);
            return Json(true);
        }
        public async Task<IActionResult> WorkDetails(int id)
        {
            return Json(await getInstance().Detail(id));
        }

        public WorkDao getInstance()
        {
            if (instance == null)
            {
                instance = WorkDao.getInstance();
            }
            return instance;
        }

        public IActionResult WeekReport()
        {
            var result = getInstance().FinishedWorks();

            return Json(result);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
