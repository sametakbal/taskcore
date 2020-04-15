using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tasky.Models;

namespace Tasky.Controllers
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

        public IActionResult Mates(string term)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            List<User> list = getContext().User.Where(w => 
            (w.Username.Contains(term) || w.Name.Contains(term) || w.Surname.Contains(term))&& w.Id != userId).ToList();

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
