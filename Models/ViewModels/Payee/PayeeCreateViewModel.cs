using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApp.Models.ViewModels.Payee
{
    public class PayeeCreateViewModel
    {
        public string Name { get; set; }
        
        public string BankAccount { get; set; }
        public string Address { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
