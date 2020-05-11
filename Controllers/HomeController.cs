using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using taskcore.Dao;
using taskcore.Models;

namespace taskcore.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext _context;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AcceptRequest(int MateId)
        {
            UserMates userMates1 = getContext().UserMates.FirstOrDefault(w => w.UserId == MateId);
            userMates1.State = State.Friend;
            getContext().Update(userMates1);
            UserMates userMates2 = new UserMates
            {
                UserId = userMates1.MateId,
                MateId = userMates1.UserId,
                State = State.Friend
            };
            getContext().Add(userMates2);
            await getContext().SaveChangesAsync();
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRequest(int MateId)
        {
            UserMates userMates = getContext().UserMates.FirstOrDefault(w => w.UserId == MateId);
            getContext().Remove(userMates);
            await getContext().SaveChangesAsync();
            return Json(true);
        }
        public async Task<IActionResult> FriendRequest(int Id)
        {
            User user = await getContext().User.FindAsync(Id);
            int? userId = HttpContext.Session.GetInt32("id");

            await getContext().UserMates.AddAsync(new UserMates
            {
                UserId = (int)userId,
                MateId = user.Id,
                State = (State)1
            });

            await getContext().SaveChangesAsync();

            return Json(true);
        }
        public async Task<IActionResult> GetFriendRequests()
        {
            int? userId = HttpContext.Session.GetInt32("id");
            var list = getContext().UserMates.Where(w => w.MateId == userId && w.State == (State)1).ToList();
            foreach (var item in list)
            {
                item.User = await getContext().User.FindAsync(item.UserId);
            }

            return Json(list);
        }
        public IActionResult GetFriendState(int Id)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            UserMates userMates = getContext().UserMates.FirstOrDefault(w => w.UserId == (int)userId && w.MateId == Id);
            if (userMates == null)
            {
                userMates = new UserMates();
            }
            return Json((int)userMates.State);
        }

        public IActionResult Mates(string term)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            List<User> list = getContext().User.Where(w =>
            (w.Username.Contains(term) || w.Name.Contains(term) || w.Surname.Contains(term)) && w.Id != userId).OrderBy(w => w.Name).ToList();

            return View(list);
        }
        public IActionResult MatesList(string term)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            List<User> list = getContext().User.ToList();
            return View(list);
        }

        public async Task<IActionResult> UserProfile(int Id)
        {
            User user = await getContext().User.FindAsync(Id);
            return Json(user);
        }

        public DatabaseContext getContext()
        {
            if (_context == null)
            {
                _context = DatabaseContext.getContext();
            }
            return _context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
