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
    public class UsersController : Controller
    {
        private readonly Parental_ControlContext _context;

        public UsersController(Parental_ControlContext context)
        {
            _context = context;
        }

        // GET: Users1
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'Parental_ControlContext.Users'  is null.");
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
        public IActionResult login(String username,String password)
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
                    return RedirectToAction("login", "Users");
                }
            }
            return View();
        }

        public IActionResult logout() {
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
                return RedirectToAction("login","Users");
        }

        // GET: Users1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,username,password,logged_in")] Users users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,username,password,logged_in")] Users users)
        {
            if (id != users.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'Parental_ControlContext.Users'  is null.");
            }
            var users = await _context.Users.FindAsync(id);
            if (users != null)
            {
                _context.Users.Remove(users);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
          return (_context.Users?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
