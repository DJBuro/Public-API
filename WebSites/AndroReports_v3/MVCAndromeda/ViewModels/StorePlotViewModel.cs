using MVCAndromeda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromedaDataAccess.Domain;

namespace MVCAndromeda.ViewModels
{
    public class StorePlotViewModel
    {
        public StorePlotData Data { get; set; }
        public MyAndromedaUser User { get; set; } 
    }
}