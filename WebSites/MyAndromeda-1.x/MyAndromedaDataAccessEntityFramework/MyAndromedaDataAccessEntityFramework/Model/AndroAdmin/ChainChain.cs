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
    
    public partial class ChainChain
    {
        public int Id { get; set; }
        public int ParentChainId { get; set; }
        public int ChildChainId { get; set; }
    
        public virtual Chain ChildChain { get; set; }
        public virtual Chain ParentChain { get; set; }
    }
}
