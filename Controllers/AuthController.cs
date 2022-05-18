using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parental_Control.Data;
using Parental_Control.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Parental_Control.Controllers
{
    public class AuthController : Controller
    {
        public readonly Parental_ControlContext _context;

        public AuthController(Parental_ControlContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (checkLogin() != true)
            {
                return View();
            }
            else
            {
                TempData["MSG"] = "Already Logged In";
                TempData["MSG_Type"] = "success";
                return RedirectToAction("", "Home");
            }
        }
        public Boolean checkLogin()
        {
            string userId = HttpContext.Session.GetString("userId");
            string userName = HttpContext.Session.GetString("username");
            if (userId != null && userName != null)
            {
                var User = _context.Users
                .Where(users => users.id == Int32.Parse(userId))
                .Where(users => users.username == userName).FirstOrDefault();
                if (User.logged_in == 1)
                {
                    return true;
                }
                else
                {
                    logout();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(String username, String password)
        {

            bool isLoggedIn = checkLogin();
            ViewBag.isAuth = isLoggedIn;
            ViewBag.userId = HttpContext.Session.GetString("userId");
            ViewBag.userName = HttpContext.Session.GetString("username");
            if (username != null && password != null && isLoggedIn != true)
            {
                var User = _context.Users
                     .Where(users => users.username == username)
                     .Where(users => users.password == password).FirstOrDefault();
                if (User != null)
                {
                    HttpContext.Session.SetString("userId", User.id.ToString());
                    HttpContext.Session.SetString("username", User.username);
                    User.logged_in = 1;
                    _context.SaveChanges();
                    return RedirectToAction("", "Home");
                }
            }
            ViewBag.StatusMsg = "Username Or Password Is Invalid";
            ViewBag.msgType = "danger";
            return View();
        }

        public IActionResult logout()
        {
            string userId = HttpContext.Session.GetString("userId");
            string username = HttpContext.Session.GetString("username");
            if (username != null && userId != null)
            {
                var User = _context.Users
             .Where(users => users.id == Int32.Parse(userId))
             .Where(users => users.username == username).FirstOrDefault();
                User.logged_in = 0;
                _context.SaveChanges();
            }
            HttpContext.Session.Clear();
            return RedirectToAction("", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("id,username,password")] Users User)
        {
            Debug.WriteLine(User);
            if (_context.Users.Where(users=>users.username == User.username).FirstOrDefault() == null)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(User);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View("Index");
        }
    }
}
