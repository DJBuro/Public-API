namespace MyAndromeda.Web.Areas.DeliveryZone.Models
{
    public class PostCodeSectorViewModel
    {
        public int Id { get; set; }

        public int DeliveryZoneId { get; set; }

        public string PostCodeSector { get; set; }

        public bool IsSelected { get; set; }

        public int DataVersion { get; set; }
    }
}