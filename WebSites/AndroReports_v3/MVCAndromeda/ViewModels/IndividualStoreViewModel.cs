using MVCAndromeda.Models;
using MyAndromedaDataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCAndromeda.ViewModels
{
    public class IndividualStoreViewModel
    {
        public MyAndromedaUser User { get; set; }
        public Store Store { get; set; }
    }
}