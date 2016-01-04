using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration.Bringg.APIModel
{
    public class BringgGetSigned
    {
        public int company_id { get; set; }
        public string access_token { get; set; }
        public string timestamp { get; set; }
        public string signature { get; set; }
    }
}
