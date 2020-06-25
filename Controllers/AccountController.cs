using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using taskcore.Dao;
using taskcore.Manager;
using taskcore.Models;

namespace taskcore.Controllers
{
    public class AccountController : Controller
    {

        private UserDao userDao = null;
        private DaOperations dao = null;
        private string code = null;

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Project/Index");
            }
            if (HttpContext.Session.GetInt32("error") == null)
            {
                HttpContext.Session.SetInt32("error", 0);
            }
            return View();
        }

        public async Task<IActionResult> ChangePassword(string current,string pass)
        {
            int uid = HttpContext.Session.GetInt32("id").Value;
            if (uid !=0 && current !=null && pass !=null)
            {
                var user = await getDao().GetUser(uid);
                if (current.Equals(user.Password))
                {
                    user.Password = pass;
                    await getDao().Modify(user);
                }

            }
            return Redirect("Index");
        }
        public IActionResult ForgotPassword()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("Index");
            }
            HttpContext.Session.SetInt32("error", 0);
            return View();
        }

        public IActionResult Friends()
        {
            return View(getDao().FriendList((int)HttpContext.Session.GetInt32("id")));
        }
        public IActionResult FriendsList()
        {
            return Json(getDao().FriendList((int)HttpContext.Session.GetInt32("id")));
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            User user = await getDao().Find(model);
            if (user != null)
            {
                HttpContext.Session.SetInt32("id", user.Id);
                HttpContext.Session.SetString("name", user.Name);
                HttpContext.Session.SetString("surname", user.Surname);
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetInt32("error", 0);
                return Redirect("/Project/Index");
            }
            HttpContext.Session.SetInt32("error", 1);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return Redirect("Index");
        }
        public IActionResult None()
        {
            HttpContext.Session.SetInt32("error", 0);
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            int? id = HttpContext.Session.GetInt32("id");
            if (!id.HasValue)
            {
                return Redirect("Index");
            }

            return View(await getDao().GetUser(id.Value));
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            await getDao().Create(user);
            return Json(true);
        }

        public async Task<IActionResult> Remove(int Id)
        {
            int? uid = HttpContext.Session.GetInt32("id");
            await getDao().RemoveFriend(uid.Value, Id);
            return Redirect("Friends");
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
       public async Task<IActionResult> SendACode(string email)
        {
            User tmp = await getDao().Find(email);

            if (tmp != null)
            {
                await MailManager.ResetPasswordCode(email,getCode());
                await getDao().InserCode(new PasswordCode{UserId=tmp.Id,Code=getCode() });
            }
            else
            {
                return Json(false);
            }
            return Redirect("ResetPassword");
        }

        public IActionResult Settings()
        {
            if (!HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("NotFound");
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

        public async Task<IActionResult> Statistics()
        {
            int? id = HttpContext.Session.GetInt32("id");
            if (!id.HasValue)
            {
                return Redirect("None");
            }
            List<ProjectsProgress> res = await ProjectDao.getInstance().ProgressList((int)id);
            return View(res);
        }



        [HttpPost]
        public async Task<IActionResult> UpdateProfile(User usr)
        {
            await getDao().Modify(usr);
            return Json(true);
        }

        public async Task<IActionResult> UpdatePassword(string Code, string Password)
        {
            PasswordCode psd = await getDao().getPasswordCode(Code);
            if (psd != null)
            {
                User user = await getDao().GetUser(psd.UserId);
                user.Password = Password;
                await getDao().Modify(user);
                await getDao().RemoveCode(psd);
            }

            return RedirectToAction(nameof(Index));
        }
        

            public  string getCode()
        {
            if (code == null)
            {
                Random random = new Random();
                code = "";
                for (int i = 0; i < 6; i++)
                {
                    char tmp = Convert.ToChar(random.Next(48, 58));
                    code += tmp;
                }
            }

            return code;
        }

  /*      public UserDao getDao()
        {
            return userDao == null ? userDao = UserDao.getInstance() : userDao;
        }*/
        
        public UserDao getDao()
        {
            return  DaOperations.GetUserDao();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
