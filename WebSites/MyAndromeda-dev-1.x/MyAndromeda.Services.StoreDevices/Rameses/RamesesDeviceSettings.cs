using System;
using System.Linq;
using System.Collections.Generic;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.AndroAdmin;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Logging;

namespace MyAndromeda.Services.StoreDevices.Rameses
{

    public class RamesesDeviceSettings 
    {
        public bool? AllowEmails { get; set; }
    }

}