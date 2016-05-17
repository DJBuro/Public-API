using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromeda.Web.Areas.Store.Controllers;

namespace MyAndromeda.Web.Areas.Store.Models
{
    public class LocalizationViewModel
    {
        public string SelectedTimeZoneId { get; set; }
        public string SelectedCultureType { get; set; }

        public IEnumerable<CultureChoiceViewModel> CultureChoices { get; set; }

        public IOrderedEnumerable<TimeZoneViewModel> TimezoneChoices { get; set; }
    }

    public class CultureChoiceViewModel 
    {
        public string Name { get; set; }
        public string EnglishName { get; set; }
    }

    public class TimeZoneViewModel 
    {
        public string DisplayName {get;set; }
        public string StandardName {get;set;}
        public string Id {get;set;}
        public string BaseUtcOffset { get; set; }
    }
}