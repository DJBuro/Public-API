//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAndromedaDataAccessEntityFramework.Model.AndroAdmin
{
    using System;
    using System.Collections.Generic;
    
    public partial class DeliveryArea
    {
        public System.Guid Id { get; set; }
        public int StoreId { get; set; }
        public string DeliveryArea1 { get; set; }
        public int DataVersion { get; set; }
        public bool Removed { get; set; }
    
        public virtual Store Store { get; set; }
    }
}
