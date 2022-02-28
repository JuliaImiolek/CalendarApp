using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApp.Models
{
    public class Event
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Payment { get; set; }
        public DateTime Notification { get; set; }
        public bool Periodicity { get; set; }

        //Realtional data
        public string PayeeId { get; set; }
        [JsonIgnore]
        public virtual Payee Payee { get; set; }
        public string ApplicationUserId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
