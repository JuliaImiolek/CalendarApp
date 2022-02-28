using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CalendarApp.Data;
using CalendarApp.Models;
using CalendarApp.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CalendarApp.Models.ViewModels.Event;

namespace CalendarApp.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EventController> _logger;

        public EventController(IDAL dal, UserManager<ApplicationUser> userManager, ILogger<EventController> logger)
        {
            _dal = dal;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Event
        public IActionResult Index()
        {
            if (TempData["Alert"] != null)
            {
                ViewData["Alert"] = TempData["Alert"];
            }
                return View(_dal.GetMyEvents(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        // GET: Event/Details/5
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)== true)
            {
                return NotFound();
            }

            var founded = _dal.GetEvent(id);
            if (founded == null)
            {
                return NotFound();
            }

            return View(founded);
        }

        // GET: Event/Create
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            
            var payees = _dal.GetMyPayees(user.Id);
            ViewBag.payees = payees;
            EventCreateViewModel eventCreateViewModel = new EventCreateViewModel()
            {
                
            };
            return View(eventCreateViewModel);
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateViewModel vm)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            DateTime StartDate;
            DateTime EndDate;
            DateTime Notification;

            if (ModelState.IsValid)
            {

                if (!DateTime.TryParseExact(vm.StartDate, @"yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out StartDate))
                {
                    ModelState.AddModelError("StartDate", "Nieprawidłowy format daty.");
                    return View(vm);
                }
                if (!DateTime.TryParseExact(vm.EndDate, @"yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out EndDate))
                {
                    ModelState.AddModelError("EndDate", "Nieprawidłowy format daty.");
                    return View(vm);
                }
                if (!DateTime.TryParseExact(vm.Notification, @"yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out Notification))
                {
                    ModelState.AddModelError("Notification", "Nieprawidłowy format daty.");
                    return View(vm);
                }

               

                await  _dal.CreateEvent(vm.Name, vm.Description, StartDate, EndDate, vm.Payment, Notification, vm.Periodicity, user.Id, vm.PayeeId);
                TempData["Alert"] = "Success! You created a new event for: " + vm.Name;
                
                if (vm.Periodicity == true)
                {
                    DateTime newStartDate = StartDate;
                    int i = 1;
                    while (newStartDate.CompareTo(EndDate) != 1)
                    {
                        newStartDate = StartDate.AddMonths(i);
                        DateTime newNotification = Notification.AddMonths(i);
                        await _dal.CreateEvent(vm.Name, vm.Description, newStartDate, EndDate, vm.Payment, newNotification, vm.Periodicity, user.Id, vm.PayeeId);

                        i++;
                    }
                    

                    
                }
                return RedirectToAction("index");
            }
            else
            {
                return View(vm);
            }

        }

        // GET: Event/Edit/5
        [UserAcessOnly]
        public IActionResult Edit(string id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var founded = _dal.GetEvent(id);
            if (founded == null)
            {
                return NotFound();
            }
            var vm = new EventEditViewModel
            {
                Name = founded.Name,
                Description = founded.Description,
                StartDate = founded.StartDate.ToString(),
                EndDate = founded.EndDate.ToString(),
                Payment = founded.Payment,
                Notification = founded.Notification.ToString(),
                Periodicity = founded.Periodicity,
                EventId = founded.Id,
                UserID = founded.ApplicationUserId
                
            };
        
            return View(vm);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventEditViewModel vm)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            DateTime StartDate;
            DateTime EndDate;
            DateTime Notification;

            if (ModelState.IsValid)
            {

                if (!DateTime.TryParseExact(vm.StartDate, @"yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out StartDate))
                {
                    ModelState.AddModelError("StartDate", "Nieprawidłowy format daty.");
                    return View(vm);
                }
                if (!DateTime.TryParseExact(vm.EndDate, @"yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out EndDate))
                {
                    ModelState.AddModelError("EndDate", "Nieprawidłowy format daty.");
                    return View(vm);
                }
                if (!DateTime.TryParseExact(vm.Notification, @"yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out Notification))
                {
                    ModelState.AddModelError("Notification", "Nieprawidłowy format daty.");
                    return View(vm);
                }



               await _dal.UpdateEvent(vm.Name, vm.Description, StartDate, EndDate, vm.Payment, Notification, vm.Periodicity,vm.UserID, vm.PayeeId, vm.EventId);
                TempData["Alert"] = "Success! You modified an event for: " + vm.Name;
                return RedirectToAction("index");
            }
            else
            {
                return View(vm);
            }

        }
       
        // GET: Event/Delete/5
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id) == true)
            {
                return NotFound();
            }

            var founded = _dal.GetEvent(id);
            if (founded == null)
            {
                return NotFound();
            }
            return View(founded);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _dal.DeleteEvent(id);
            TempData["Alert"] = "You delated an event.";
            return RedirectToAction(nameof(Index));
        }

        //private bool EventExists(int id)
        //{
        //    return _context.Events.Any(e => e.Id == id);
        //}
    }
}
