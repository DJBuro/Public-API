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
    
    public partial class AMSServerChain
    {
        public int Id { get; set; }
        public int AMSServerId { get; set; }
        public int ChainId { get; set; }
    
        public virtual AMSServer AMSServer { get; set; }
        public virtual Chain Chain { get; set; }
    }
}
