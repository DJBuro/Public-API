using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromedaDataAccess.Domain;

namespace MyAndromeda.Web.ViewModels
{
    public class ChainUpdateCultureViewModel
    {
        public string ReturnUrl { get; set; }
        public Chain Chain { get; set; }
    }
}