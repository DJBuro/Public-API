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
    
    public partial class DeliveryAreaTown
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Postcodes { get; set; }
    
        public virtual ACSApplication ACSApplication { get; set; }
    }
}
