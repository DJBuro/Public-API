using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Menus.Models
{
    public class MenuStatus
    {
        public string ExternalStoreName { get; set; }
        public string ClientSiteName { get; set; }

        public DateTime? LastPublished { get; set; }
    }

    public class MenuJobModel
    {
        public SiteMenu Menu { get; set; }
        public bool RanToCompetion { get; set; }
        public bool Success { get; set; }
    }
}
