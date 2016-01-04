using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAndromeda.Web.Areas.DeliveryZone.Models
{
    public class DeliveryZoneNameViewModel
    {
        public DeliveryZoneNameViewModel()
        {
            this.PostCodeSectors = new List<PostCodeSectorViewModel>();
        }

        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; }
        public decimal? RadiusCovered { get; set; }
        public string OriginPostCode { get; set; }
        public bool? IsCustom { get; set; }
        public List<PostCodeSectorViewModel> PostCodeSectors { set; get; }
    }
}