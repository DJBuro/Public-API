//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAndromeda.Data.DataWarehouse.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDiscountType
    {
        public OrderDiscountType()
        {
            this.OrderDiscounts = new HashSet<OrderDiscount>();
        }
    
        public string Type { get; set; }
    
        public virtual ICollection<OrderDiscount> OrderDiscounts { get; set; }
    }
}
