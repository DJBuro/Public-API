namespace AndroCloudServices.Models
{
    using System.Collections.Generic;

    public sealed class AcsSiteDetails
    {
        public string SiteId { get; set; }

        public string Name { get; set; }

        public int? EstimatedDeliveryDuration { get; set; }

        public int? EstimatedCollectionDuration { get; set; }

        public string TimeZone { get; set; }

        public string Phone { get; set; }

        public AcsSiteAddress Address { get; set; }

        public ICollection<AcsOpeningHour> OpeningHours { get; set; }

        public ICollection<string> DeliveryZones { get; set; }
    }
}