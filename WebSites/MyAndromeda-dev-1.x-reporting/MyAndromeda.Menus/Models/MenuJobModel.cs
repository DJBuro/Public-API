using System;
using System.Linq;
using System.Collections.Generic;
using MyAndromeda.Data.Model.MyAndromeda;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Menus.Models
{

    public class MenuJobModel
    {
        public SiteMenu Menu { get; set; }
        public bool RanToCompetion { get; set; }
        public bool Success { get; set; }
    }

}