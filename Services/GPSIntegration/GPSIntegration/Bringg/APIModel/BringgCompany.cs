using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration.Bringg.APIModel
{
    public class BringgCompany
    {
        public bool test { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
        public string phone { get; set; }
        public string timestamp { get; set; }
        public string access_token { get; set; }
    }
}
