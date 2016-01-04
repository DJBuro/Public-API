using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration.Model
{
    public class Driver
    {
        /// <summary>
        /// The Bringg driver id.  Leave it blank - it gets automatically set to the existing or new Bringg driver id
        /// </summary>
        public int? ExternalId { get; set; }

        /// <summary>
        /// The driver name.  Required
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The drivers phone number.  Required.  Bringg integration requires a valid phone number for each driver
        /// </summary>
        public string Phone { get; set; }
    }
}
