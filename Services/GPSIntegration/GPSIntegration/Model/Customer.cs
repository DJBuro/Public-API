using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration.Model
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string PartnerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
}
