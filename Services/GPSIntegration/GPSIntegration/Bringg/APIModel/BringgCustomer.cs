using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration.Bringg.APIModel
{
    public class BringgCustomer
    {
        public string name { get; set; }
        public int company_id { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
        public string external_id { get; set; }
        public bool allow_login { get; set; }
        public string confirmation_code { get; set; }
        public string access_token { get; set; }
        public string timestamp { get; set; }
    }
}
