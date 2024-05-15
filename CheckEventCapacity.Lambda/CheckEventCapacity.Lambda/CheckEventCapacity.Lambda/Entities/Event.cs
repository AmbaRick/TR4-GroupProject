using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckEventCapacity.Lambda.Entities
{
    public class Event
    {
        public string Id { get; set; }
        public string eventName { get; set; }
        public int capacity { get; set; }
    }
}
