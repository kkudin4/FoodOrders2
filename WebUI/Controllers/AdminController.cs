using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly FoodContext _context;


         bool CheckUser()
        {
            if (HttpContext.Session.GetString("IsAdmin") == null || HttpContext.Session.GetString("IsAdmin") == "0")
            {
                return false;
            }
            return true;
        }

        public AdminController(FoodContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string secret)
        {
            if (secret == Startup.AdminPsw)
            {
                HttpContext.Session.SetString("IsAdmin", "1");
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ErrorMsg = "Неверный пароль";

            return View();
        }


        public async Task<IActionResult> LogOut([Bind("secret")] string secret)
        {

                HttpContext.Session.SetString("IsAdmin", "0");
                return RedirectToAction(nameof(Index));
   
        }



        // GET: Admin
        public async Task<IActionResult> Index()
        {
            if (!CheckUser())
                return RedirectToAction("Login", "Admin");
            return View(await _context.Order.ToListAsync());
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!CheckUser())
                return RedirectToAction("Login", "Admin");
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            if (!CheckUser())
                return RedirectToAction("Login", "Admin");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,Address,Phone,Posted,FoodName")] Order order)
        {
            if (!CheckUser())
                return RedirectToAction("Login", "Admin");
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!CheckUser())
                return RedirectToAction("Login", "Admin");
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,Address,Phone,Posted,FoodName")] Order order)
        {
            if (!CheckUser())
                return RedirectToAction("Login", "Admin");
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            return View(order);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!CheckUser())
                return RedirectToAction("Login", "Admin");
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!CheckUser())
                return RedirectToAction("Login", "Admin");
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
