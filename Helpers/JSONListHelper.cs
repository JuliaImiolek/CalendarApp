using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApp.Helpers
{
    public static class JSONListHelper
    {
        public static string GetEventListJSONString(List<Models.Event> events)
        {
            var eventlist = new List<Event>();
            foreach (var model in events)
            {
                var myevent = new Event()
                {
                   // id = int.Parse(model.Id),
                    title = model.Name,
                    description = model.Description,
                    start = model.StartDate,
                    notification = model.Notification,
                   // resourceId = int.Parse(model.Payee.Id)


                };
                eventlist.Add(myevent);
            }
            return System.Text.Json.JsonSerializer.Serialize(eventlist);
            //return JsonConvert.SerializeObject(events.ToArray());

        }

        public static string GetResourceListJSONString(List<Models.Payee> payees)
        {
            var resourcelist = new List<Resource>();

            foreach (var pay in payees)
            {
                var resource = new Resource()
                {
                   // id = int.Parse(pay.Id),
                    title = pay.Name
                };
                resourcelist.Add(resource);
            }
            return System.Text.Json.JsonSerializer.Serialize(resourcelist);
          //  return JsonConvert.SerializeObject(payees.ToArray());
        }
    }
     
    public class Event
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime notification { get; set; }
        public int resourceId { get; set; }
        public string description { get; set; }
    }

    public class Resource
    {
        public int id { get; set; }
        public string title { get; set; }

    }
}

