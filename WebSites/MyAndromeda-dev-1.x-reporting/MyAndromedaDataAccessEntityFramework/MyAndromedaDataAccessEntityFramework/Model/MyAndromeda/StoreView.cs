//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAndromedaDataAccessEntityFramework.Model.MyAndromeda
{
    using System;
    using System.Collections.Generic;
    
    public partial class StoreView
    {
        public StoreView()
        {
            this.StoreEnrolments = new HashSet<StoreEnrolment>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int AndromedaSiteId { get; set; }
        public string CustomerSiteId { get; set; }
        public int StoreStatusId { get; set; }
        public string ClientSiteName { get; set; }
        public string ExternalSiteName { get; set; }
        public string ExternalId { get; set; }
        public Nullable<int> EstimatedDeliveryTime { get; set; }
        public string TimeZone { get; set; }
        public string Telephone { get; set; }
        public string LicenseKey { get; set; }
        public int AddressId { get; set; }
        public int DataVersion { get; set; }
        public int ChainId { get; set; }
    
        public virtual ICollection<StoreEnrolment> StoreEnrolments { get; set; }
    }
}
