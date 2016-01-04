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
    // Groups_Ext
    internal partial class GroupsExtConfiguration : EntityTypeConfiguration<GroupsExt>
    {
        public GroupsExtConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Groups_Ext");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.UserDef1).HasColumnName("UserDef1").IsOptional().HasMaxLength(64);
            Property(x => x.UserDef2).HasColumnName("UserDef2").IsOptional().HasMaxLength(64);
            Property(x => x.UserDef3).HasColumnName("UserDef3").IsOptional().HasMaxLength(64);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
