using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SendProcessingEmailLambda
{
    public class EventBooking
    {
        [JsonPropertyName("eventId")]
        public string? EventId { get; set; }

        [JsonPropertyName("eventName")]
        public string? EventName { get; set; }

        [JsonPropertyName("emailAddress")]
        public string? EmailAddress { get; set; }

        [JsonPropertyName("seats")]
        public int? Seats { get; set; }


    }
}
