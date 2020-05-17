using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using taskcore.Dao;
using taskcore.Models;

namespace taskcore.Controllers
{
    [UserFilter]
    public class ProjectController : Controller
    {
        private DaOperations instance = null;


        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> AcceptRequest(int Id)
        {
            int? uid = HttpContext.Session.GetInt32("id");
            await getInstance().ProjectAccept(Id, uid.Value);
            return Json(true);
        }

        public async Task<IActionResult> DeleteRequest(int Id)
        {
            int? uid = HttpContext.Session.GetInt32("id");

            await getInstance().ProjectDecline(Id,uid.Value);
            return Json(true);
        }

        public async Task<IActionResult> UpdateStatus(int itemid, int statusid)
        {
            await getInstance().ProjectModifyStatus(itemid, statusid);
            return Json(true);
        }
        [HttpGet]
        public IActionResult ReadToAll()
        {
            int? userId = HttpContext.Session.GetInt32("id");
            List<Project> result = getInstance().ProjectRead((userId.Value));
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            int? userId = HttpContext.Session.GetInt32("id");

            await getInstance().ProjectInsert(project, new UserProjects
            {
                UserId = userId.Value,
                IsAccept = true
            });

            return Json(true);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await getInstance().ProjectErase(id);
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Project project)
        {
            await getInstance().ProjectModify(project);
            return Json(true);
        }
        public async Task<IActionResult> ProjectDetails(int id)
        {
            var project = await getInstance().ProjectDetail(id);
            return Json(project);
        }
        [HttpPost]
        public async Task<IActionResult> ProjectRequest(int id, int projectId)
        {
            await getInstance().ProjectRequest(id, projectId);
            return Json(true);
        }

        public async Task<IActionResult> RequestList()
        {
            int? uid = HttpContext.Session.GetInt32("id");

            return Json(await getInstance().ProjectRequestList(uid.Value));
        }

        public DaOperations getInstance()
        {
            if (instance == null)
            {
                instance = DaOperations.GetInstance();
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
