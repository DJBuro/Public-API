//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AndroAdminDataAccess.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class AndroWebOrderingTheme
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AndroWebOrderingTheme()
        {
            this.AndroWebOrderingWebsites = new HashSet<AndroWebOrderingWebsite>();
        }
    
        public int Id { get; set; }
        public string ThemePath { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string ThemeName { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AndroWebOrderingWebsite> AndroWebOrderingWebsites { get; set; }
    }
}
