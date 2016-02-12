//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrderTracking.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Site
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Site()
        {
            this.ACSApplicationSites = new HashSet<ACSApplicationSite>();
        }
    
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> SignalRConnectionID { get; set; }
        public Nullable<System.Guid> SessionID { get; set; }
        public string ExternalSiteName { get; set; }
        public int AndroID { get; set; }
        public Nullable<System.Guid> AddressID { get; set; }
        public Nullable<bool> StoreConnected { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string ExternalId { get; set; }
        public Nullable<int> EstimatedDeliveryTime { get; set; }
        public string TimeZone { get; set; }
        public string Telephone { get; set; }
        public string LicenceKey { get; set; }
        public string HardwareKey { get; set; }
        public System.Guid SiteStatusID { get; set; }
        public Nullable<int> StorePaymentProviderID { get; set; }
        public Nullable<System.Guid> SiteOfflineID { get; set; }
        public string TimeZoneInfoId { get; set; }
        public string UiCulture { get; set; }
        public Nullable<int> EstimatedCollectionTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACSApplicationSite> ACSApplicationSites { get; set; }
    }
}
