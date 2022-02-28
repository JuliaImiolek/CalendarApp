using CalendarApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApp.Data
{
    public interface IDAL
    {
        public List<Event> GetEvents();
        public List<Event> GetMyEvents(string userid);
        public Event GetEvent(string id);
        public Task CreateEvent(string Name, string Description, DateTime StartDate, DateTime EndDate, double Payment, DateTime Notification, bool Periodicity, string userID, string payeeID);
        public Task UpdateEvent(string Name, string Description, DateTime StartDate, DateTime EndDate, double Payment, DateTime Notification, bool Periodicity, string userID, string payeeID, string EventId);
        public void DeleteEvent(string id);
        public List<Payee> GetPayees();
        public Payee GetPayee(string id);
        public List<Payee> GetMyPayees(string userid);
        public Task CreatePayee(string Name, string BankAccount, string Address, string userID);
        public void UpdatePayee(Payee payee);
        public void DeletePayee(string id);
       

    }
    public class DAL : IDAL
    {
        private ApplicationDbContext db = new ApplicationDbContext();
      
        public async Task CreateEvent(string Name, string Description, DateTime StartDate, DateTime EndDate, double Payment, DateTime Notification, bool Periodicity, string userID, string payeeID)
        {

            Event newEvent = new Event()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                Description = Description,
                StartDate = StartDate,
                EndDate = EndDate,
                Payment = Payment,
                Notification = Notification,
                Periodicity = Periodicity,
                ApplicationUserId = userID,
                PayeeId = payeeID
            };

            await db.Events.AddAsync(newEvent);
            await db.SaveChangesAsync();
        }

        public async Task CreatePayee(string Name, string BankAccount, string Address, string userID)
        {
            Payee newPayee = new Payee()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                BankAccount = BankAccount,
                Address = Address,
                ApplicationUserId = userID
            };
            await  db.Payees.AddAsync(newPayee);
            await db.SaveChangesAsync();
        }

        public void DeleteEvent(string id)
        {
            var _event = db.Events.Find(id);
            db.Events.Remove(_event);
            db.SaveChanges();
        }

        public void DeletePayee(string id)
        {
            var _payee = db.Payees.Find(id);
            db.Payees.Remove(_payee);
            db.SaveChanges();
        }

        public Event GetEvent(string id)
        {
            return db.Events.FirstOrDefault(x => x.Id == id);
        }

        public List<Event> GetEvents()
        {
            return db.Events.ToList();
        }

        public List<Event> GetMyEvents(string userid)
        {
            return db.Events.Where(x => x.ApplicationUser.Id == userid).ToList();
        }

        public Payee GetPayee(string id)
        {
            return db.Payees.FirstOrDefault(x => x.Id == id);
        }

        public List<Payee> GetPayees()
        {
            return db.Payees.ToList();
        }
        public List<Payee> GetMyPayees(string userid)
        {
            return db.Payees.Where(x=> x.ApplicationUser.Id == userid).ToList();
        }

        public async Task UpdateEvent(string Name, string Description, DateTime StartDate, DateTime EndDate, double Payment, DateTime Notification, bool Periodicity, string userID, string payeeID, string EventID)
        {
            Event newEvent = new Event()
            {
                Id = EventID,
                Name = Name,
                Description = Description,
                StartDate = StartDate,
                EndDate = EndDate,
                Payment = Payment,
                Notification = Notification,
                Periodicity = Periodicity,
                ApplicationUserId = userID,
                PayeeId = payeeID
            };

            db.Update(newEvent);

            await db.SaveChangesAsync();
        }

        public void UpdatePayee(Payee payee)
        {
            var payeeid = payee.Id;
            var _payee = db.Payees.FirstOrDefault(x => x.Id == payeeid);
            _payee.UpdatePayee(payee, _payee);
            db.Entry(_payee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

      
    }
}
