//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAndromeda.Data.Model.MyAndromeda
{
    using System;
    using System.Collections.Generic;
    
    public partial class MenuItemThumbnail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MenuItemThumbnail()
        {
            this.MenuItems = new HashSet<MenuItem>();
        }
    
        public System.Guid Id { get; set; }
        public string Src { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Alt { get; set; }
        public string FileName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
