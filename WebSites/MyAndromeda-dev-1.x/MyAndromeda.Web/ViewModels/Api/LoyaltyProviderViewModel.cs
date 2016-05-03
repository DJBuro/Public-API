using MyAndromeda.Data.Model.AndroAdmin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.ViewModels.Api
{

    public class LoyaltyProviderViewModel
    {
        public Guid? Id { get; set; }
        public dynamic Configuration { get; set; }
        public string ProviderName { get; set; }
    }

}