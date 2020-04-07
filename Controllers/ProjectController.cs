using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View();
        }

 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
