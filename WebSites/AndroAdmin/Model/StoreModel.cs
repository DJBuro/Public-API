using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using AndroAdminDataAccess.Domain;
using AndroAdmin.ViewModels.StoreType;

namespace AndroAdmin.Model
{
    public class StoreModel
    {
        public static SortedList<string, RegionInfo> countries;
 
        public Store Store { get; set; }
        public Address Address { get; set; }

        public IList<StoreStatus> StoreStatuses { get; set; }
        public List<Country> Countries { get; set; }
        public IList<StorePaymentProvider> StorePaymentProviders { get; set; }
        public IList<Chain> Chains { get; set; }
        
        public int PaymentProviderId { get; set; }
        public int? ChainId { get; set; }
        public int StoreStatusId { get; set; }
        public int CountryId { get; set; }
        public bool Selected { get; set; }

        public Dictionary<string, List<TimeSpanBlock>> OpeningTimesByDay { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDay { get; set; }
        public bool ChangeIsOpen { get; set; }
        public Dictionary<string, bool> IsOpen { get; set; }

        public string AddOpeningStartTime { get; set; }
        public string AddOpeningEndTime { get; set; }

        public bool ShowEditOpeningTimes { get; set; }
        
        public StoreModel()
        {
            this.Selected = false;
            this.ShowEditOpeningTimes = false;
        }
    }
}