// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

namespace MyAndromeda.Data.DailyReporting.Model.CodeFirst
{
    // Chains
    public partial class Chains
    {
        public string Name { get; set; } // Name
        public int? ParentId { get; set; } // ParentID
        public int ChainId { get; set; } // ChainID (Primary key)

        // Reverse navigation
        public virtual ICollection<Chains> Chains2 { get; set; } // Chains.FK_Chains_Chains

        // Foreign keys
        public virtual Chains Chains1 { get; set; } //  FK_Chains_Chains

        public Chains()
        {
            Chains2 = new List<Chains>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
