namespace CheckEventCapacity.Lambda.Entities
{

    /// <summary>
    /// Event properties
    /// </summary>
    public class Event
    {
        public string Id { get; set; }
        public string eventName { get; set; }
        public int capacity { get; set; }
    }
}
