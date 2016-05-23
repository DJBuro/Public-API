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
    
    public partial class Device
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Device()
        {
            this.StoreDevices = new HashSet<StoreDevice>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public int DataVersion { get; set; }
        public Nullable<System.Guid> ExternalApiId { get; set; }
        public bool Removed { get; set; }
    
        public virtual ExternalApi ExternalApi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoreDevice> StoreDevices { get; set; }
    }
}
