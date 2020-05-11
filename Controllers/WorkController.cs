using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using taskcore.Dao;
using taskcore.Models;

namespace taskcore.Controllers
{
    [UserFilter]
    public class WorkController : Controller
    {
        private WorkDao instance = null;
        private UserDao udao = null;
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
            int? uid = HttpContext.Session.GetInt32("id");
            List<User> friends = GetUserDao().FriendList(uid.Value);
            List<SelectListItem> flist = new List<SelectListItem>();
            foreach (var user in friends)
            {
                SelectListItem tmp = new SelectListItem
                {
                    Text = user.Name + " " + user.Surname,
                    Value = user.Id.ToString()
                };
                flist.Add(tmp);
            }
            ViewBag.Friends = flist;
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

        [HttpGet]
        public async Task<IActionResult> List(int Id)
        {
            int? userId = HttpContext.Session.GetInt32("id");
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

        public UserDao GetUserDao()
        {
            return udao == null ? udao = UserDao.getInstance() : udao;
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
