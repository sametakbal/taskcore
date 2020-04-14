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
    public class AccountController : Controller
    {

        private readonly DatabaseContext _context;
        public AccountController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Project/Index");
            }
            return View();
        }
        public IActionResult SignUp()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Project/Index");
            }
            return View();
        }

        public IActionResult Profile()
        {
            if (!HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("Index");
            }
            return View();
        }

        public IActionResult ForgotPassword()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("Index");
            }
            return View();
        }

        public IActionResult None()
        {
            return View();
        }

        public IActionResult Settings()
        {
            if (!HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("NotFound");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            var user = await _context.User.FirstOrDefaultAsync(w => (w.Email == model.Email || w.Username == model.Email)
            && w.Password == model.Password);
            if (user != null)
            {

                HttpContext.Session.SetInt32("id", user.Id);
                HttpContext.Session.SetString("name", user.Name);
                HttpContext.Session.SetString("surname", user.Surname);
                HttpContext.Session.SetString("username", user.Username);
                return Redirect("/Project/Index");

            }

            ViewBag.LoginError = true;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return Redirect("Index");
        }
        public async Task<IActionResult> Register(User user)
        {
            if (user != null)
            {
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
