using CalendarApp.Data;
using CalendarApp.Helpers;
using CalendarApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CalendarApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDAL _idal;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IDAL idal, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _idal = idal;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
           // ViewData["Resources"] = JSONListHelper.GetResourceListJSONString(_idal.GetPayees());
           // ViewData["Events"] = JSONListHelper.GetEventListJSONString(_idal.GetEvents());
          
            return View();
        }
        [Authorize]
        public IActionResult MyCalendar()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Resources"] = JSONListHelper.GetResourceListJSONString(_idal.GetPayees());
            ViewData["Events"] = JSONListHelper.GetEventListJSONString(_idal.GetMyEvents(userid));
            List<Models.Event> myEvents = _idal.GetMyEvents(userid);
            DateTime notification;
            DateTime today = DateTime.Today;
            TempData["alertMessage"] = null;
            foreach (var item in myEvents)
            {
               notification = item.Notification;
                if (notification.CompareTo(today) == 0)
                {
                    TempData["alertMessage"] = "Uwaga! Przypomnienie o platnosci " + item.Name + "! " + item.Description;
                   // return View();
                }
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ViewResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View("PageNotFound");
        }
    }
}
