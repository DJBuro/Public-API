using AndroAdminDataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroAdmin.Model
{
    public class StoreBringgConfigModel
    {
        public Store Store { get; set; }
        public Andromeda.GPSIntegration.Bringg.GPSConfig GPSConfig { get; set; }    
    }
}
