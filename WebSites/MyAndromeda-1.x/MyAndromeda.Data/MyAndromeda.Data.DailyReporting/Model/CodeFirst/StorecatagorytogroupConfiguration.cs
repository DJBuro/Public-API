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
    // storecatagorytogroup
    internal partial class StorecatagorytogroupConfiguration : EntityTypeConfiguration<Storecatagorytogroup>
    {
        public StorecatagorytogroupConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".storecatagorytogroup");
            HasKey(x => x.Recordid);

            Property(x => x.Recordid).HasColumnName("recordid").IsRequired();
            Property(x => x.Autoid).HasColumnName("autoid").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Mapid).HasColumnName("mapid").IsRequired();
            Property(x => x.Catagoryid).HasColumnName("catagoryid").IsRequired();
            Property(x => x.Groupid).HasColumnName("groupid").IsRequired();
            Property(x => x.Entrydate).HasColumnName("entrydate").IsRequired();
            Property(x => x.Editdate).HasColumnName("editdate").IsRequired();
            Property(x => x.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
            Property(x => x.Machine).HasColumnName("machine").IsRequired().HasMaxLength(100);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
