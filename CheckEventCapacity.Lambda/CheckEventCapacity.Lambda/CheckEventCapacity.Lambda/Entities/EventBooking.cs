using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckEventCapacity.Lambda.Entities
{
    public class EventBooking
    {
        public string eventId { get; set; }
        public string eventName { get; set; }
        public string emailAddress { get; set; }
        public int seats { get; set; }
    }
}
