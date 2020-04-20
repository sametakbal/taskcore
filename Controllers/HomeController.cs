using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using taskcore.Dao;
using taskcore.Models;

namespace taskcore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DatabaseContext _context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserProfile(int Id)
        {
            User user = await getContext().User.FindAsync(Id);
            return Json(user);
        }

        public IActionResult GetFriendState(int Id)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            UserMates userMates = getContext().UserMates.FirstOrDefault(w => (w.UserId == userId && w.MateId == Id) || (w.MateId == Id && w.UserId == userId));
            if (userMates == null)
            {
                userMates = new UserMates();
            }
            return Json((int)userMates.State);
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

        public IActionResult Mates(string term)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            List<User> list = getContext().User.Where(w =>
            (w.Username.Contains(term) || w.Name.Contains(term) || w.Surname.Contains(term)) && w.Id != userId).OrderBy(w => w.Name).ToList();

            return View(list);
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
