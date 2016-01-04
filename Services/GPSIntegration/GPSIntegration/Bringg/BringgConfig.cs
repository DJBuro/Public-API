using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration.Bringg
{
    public class GPSConfig
    {
        public string partnerName { get; set; }
        public bool isEnabled { get; set; }
        public BringgConfig partnerConfig { get; set; }
    }

    public class BringgConfig
    {
        public string apiUrl { get; set; }
        public int? companyId { get; set; }
        public string accessToken { get; set; }
        public string secretKey { get; set; }
        public bool apiCallsEnabled { get; set; }
        public bool testMode { get; set; }
        public string clockInAPIUrl { get; set; }
    }
}
