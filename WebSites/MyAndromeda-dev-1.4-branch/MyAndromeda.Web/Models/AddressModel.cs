using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromedaDataAccess.Domain;
using System.ComponentModel.DataAnnotations;

namespace MyAndromeda.Web.Models
{
    public class AddressModel
    {
        public Address Address { get; set; }
        
        public string ExternalSiteId { get; set; }

        public int CountryId { get; set; }

        public List<Country> Countries { get; set; }

        public string ClientSiteName { get; set; }
    }
}