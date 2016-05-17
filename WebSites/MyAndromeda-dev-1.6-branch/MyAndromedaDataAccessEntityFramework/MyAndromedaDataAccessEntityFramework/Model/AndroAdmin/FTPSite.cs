//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAndromeda.Data.Model.AndroAdmin
{
    using System;
    using System.Collections.Generic;
    
    public partial class FTPSite
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FTPSite()
        {
            this.FTPSiteChains = new HashSet<FTPSiteChain>();
            this.StoreAMSServerFtpSites = new HashSet<StoreAMSServerFtpSite>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Port { get; set; }
        public string ServerType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsPrimary { get; set; }
        public int FTPSiteType_Id { get; set; }
    
        public virtual FTPSiteType FTPSiteType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FTPSiteChain> FTPSiteChains { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoreAMSServerFtpSite> StoreAMSServerFtpSites { get; set; }
    }
}
