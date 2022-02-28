using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CalendarApp.Data;
using CalendarApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CalendarApp.Models.ViewModels.Payee;
using System.Security.Claims;

namespace CalendarApp.Controllers
{
    [Authorize]
    public class PayeeController : Controller
    {
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _userManager;

        public PayeeController(IDAL dal, UserManager<ApplicationUser> userManager)
        {
            _dal = dal;
            _userManager = userManager;
        }

        // GET: Payee
        public IActionResult Index()
        {
            if (TempData["Alert"] != null)
            {
                ViewData["Alert"] = TempData["Alert"];
            }
            return View(_dal.GetMyPayees(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        // GET: Payee/Details/5
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id) == true)
            {
                return NotFound();
            }

            var payee = _dal.GetPayee(id);
            if (payee == null)
            {
                return NotFound();
            }

            return View(payee);
        }

        // GET: Payee/Create
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View(new PayeeCreateViewModel());
        }

        // POST: Payee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PayeeCreateViewModel vm)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                try
                {
                    await _dal.CreatePayee(vm.Name, vm.BankAccount, vm.Address, user.Id);
                    TempData["Alert"] = "Success! You created a location for: " + vm.Name;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(vm);
                }
            }
            return View(vm);
        }

        // GET: Payee/Edit/5
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id) == true)
            {
                return NotFound();
            }

            var payee = _dal.GetPayee(id);
            if (payee == null)
            {
                return NotFound();
            }
            return View(payee);
        }

        // POST: Payee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Name,BankAccount,Address")] Payee payee)
        {
            if (id != payee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dal.UpdatePayee(payee);
                    TempData["Alert"] = "Success! You modified an event for: " + payee.Name;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewData["Alert"] = "An error occured: " + ex.Message;

                    return View(payee);
                }
            }
            return View(payee);
        }

        // GET: Payee/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payee = _dal.GetPayee(id);
                
            if (payee == null)
            {
                return NotFound();
            }

            return View(payee);
        }

        // POST: Payee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _dal.DeletePayee(id);
            TempData["Alert"] = "You delated an payee.";
            return RedirectToAction(nameof(Index));
        }

        //private bool PayeeExists(int id)
        //{
        //    return _context.Payees.Any(e => e.Id == id);
        //}
    }
}
