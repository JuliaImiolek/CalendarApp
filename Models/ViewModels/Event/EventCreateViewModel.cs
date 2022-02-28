using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApp.Models.ViewModels
{
    public class EventCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public double Payment { get; set; }
        public string Notification { get; set; }
        public bool Periodicity { get; set; }
        public string PayeeId { get; set; }
        
    }
}
