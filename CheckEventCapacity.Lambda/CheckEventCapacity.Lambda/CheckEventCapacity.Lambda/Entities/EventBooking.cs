namespace CheckEventCapacity.Lambda.Entities
{

    /// <summary>
    /// Event Booking properties
    /// </summary>
    public class EventBooking
    {
        public string eventId { get; set; }
        public string eventName { get; set; }
        public string emailAddress { get; set; }
        public int seats { get; set; }

    }
}
