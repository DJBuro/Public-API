using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Reporting.ViewModels
{
    public class ContactViewModel
    {
        public string Value { get; set; }

        public string ContactTypeName { get; set; }

        public string MarketingLevelName { get; set; }

        public int Id { get; set; }
    }

    public static class ContactViewModelExtensions 
    {
        public static ContactViewModel ToViewModel(this Contact contact) 
        {
            return new ContactViewModel()
            {
                Id = contact.Id,
                Value = contact.Value,
                ContactTypeName = contact.ContactType.Name,
                MarketingLevelName = contact.MarketingLevel.Name
            };
        }
    }
}