//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAndromeda.Data.DataWarehouse.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderStatusHistory
    {
        public System.Guid Id { get; set; }
        public System.Guid OrderHeaderId { get; set; }
        public int Status { get; set; }
        public System.DateTime ChangedDateTime { get; set; }
    
        public virtual OrderStatu OrderStatu { get; set; }
    }
}
