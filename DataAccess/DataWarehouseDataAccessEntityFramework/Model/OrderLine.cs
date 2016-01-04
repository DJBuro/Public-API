//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataWarehouseDataAccessEntityFramework.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderLine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderLine()
        {
            this.modifiers = new HashSet<modifier>();
        }
    
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> OrderHeaderID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string Description { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<int> Price { get; set; }
        public string Cat1 { get; set; }
        public string Cat2 { get; set; }
        public string DisplayCategory { get; set; }
        public Nullable<bool> IsDeal { get; set; }
        public Nullable<System.Guid> DealID { get; set; }
        public Nullable<int> DealSequence { get; set; }
        public string Person { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<modifier> modifiers { get; set; }
        public virtual OrderHeader OrderHeader { get; set; }
    }
}
