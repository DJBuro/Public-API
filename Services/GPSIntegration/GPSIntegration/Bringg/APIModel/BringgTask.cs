using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration.Bringg.APIModel
{
    public class BringgNote 
    {
     //   public int task_id { get; set; }
    //    public int? way_point_id { get; set; }
        public string note { get; set; }
        public int company_id { get; set; }
        //public int user_id { get; set; }
        public int type { get; set; }
        public string access_token { get; set; }
        public string timestamp { get; set; }
    }
    
    public class BringgTask
    {
        public int? id { get; set; } 
        public int? customer_id { get; set; }
        public int? company_id { get; set; }
        public int? user_id { get; set; }
        public string title { get; set; }
        public int? team_id { get; set; }
        public bool silent { get; set; }
        public string note { get; set; }
        public string scheduled_at { get; set; }
        public bool asap { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
        public string address { get; set; }
        public string extras { get; set; }
        public string external_id { get; set; } // Andromeda order Id
        public int? place_id { get; set; }
        public decimal? total_price { get; set; } // Everything - including delivery!
        public decimal? left_to_be_paid { get; set; }
        public decimal? delivery_price { get; set; }
        public int priority { get; set; }
        public decimal? price_before_tax { get; set; }
        public decimal? tax_price { get; set; }
        public string access_token { get; set; }
        public string timestamp { get; set; }
    }

    public class BringgTaskDetailModel : BringgTask
    {
        //lets see if you are here - null sometimes 
        public string active_way_point_id { get; set; }

        public List<BringgWaypoint> way_points { get; set; }
    }

    public class BringgWaypoint 
    {
        public string id { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }

        public string address { get; set; }
        public int? customer_id { get; set; }
    }
}
