using System;

namespace CloudSyncModel
{
    public class StoreOccasionTimeModel 
    {
        public int AndromedaSiteId { get; set; }
        public bool Deleted { get; set; }
        public Guid Id { get; set; }
        public DateTime EndUtc { get; set; }
        public bool IsAllDay { get; set; }
        public string Occasions { get; set; }
        public string RecurrenceException { get; set; }
        public string RecurrenceRule { get; set; }
        public DateTime StartUtc { get; set; }
        public string Title { get; set; }
    }
}