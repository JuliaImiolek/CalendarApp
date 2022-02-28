using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApp.Models
{
    public class Payee
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string BankAccount { get; set; }
        public string Address { get; set; }

        // Relational data
        [JsonIgnore]
        public virtual ICollection<Event> Events { get; set; }
        public string ApplicationUserId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public void UpdatePayee(Payee payee, Payee _payee)
        {
            Id = _payee.Id;
            Name = payee.Name;
            BankAccount = payee.BankAccount;
            Address = payee.Address;
        }
    }
   
}
