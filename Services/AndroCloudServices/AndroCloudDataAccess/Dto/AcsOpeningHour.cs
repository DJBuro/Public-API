namespace AndroCloudServices.Models
{
    using System;

    public sealed class AcsOpeningHour
    {
        public string Day { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool OpenAllDay { get; set; }
    }
}