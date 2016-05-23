using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Web.Models
{
    public class AddressModel
    {
        public AddressDomainModel Address { get; set; }
        
        public string ExternalSiteId { get; set; }

        public int CountryId { get; set; }

        public List<CountryDomainModel> Countries { get; set; }

        public string ClientSiteName { get; set; }
    }
}