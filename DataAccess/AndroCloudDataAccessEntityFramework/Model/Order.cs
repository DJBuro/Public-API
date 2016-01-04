//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AndroCloudDataAccessEntityFramework.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> SiteID { get; set; }
        public Nullable<System.Guid> ACSQueueID { get; set; }
        public Nullable<System.Guid> StatusId { get; set; }
        public Nullable<int> InternetOrderNumber { get; set; }
        public string ExternalID { get; set; }
        public Nullable<int> ApplicationID { get; set; }
    
        public virtual ACSQueue ACSQueue { get; set; }
        public virtual OrderStatu OrderStatu { get; set; }
        public virtual Site Site { get; set; }
    }
}
