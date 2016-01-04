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
    // Groups
    internal partial class GroupsConfiguration : EntityTypeConfiguration<Groups>
    {
        public GroupsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Groups");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("id").IsRequired();
            Property(x => x.NUid).HasColumnName("nUID").IsOptional();
            Property(x => x.StrName).HasColumnName("strName").IsOptional().HasMaxLength(50);
            Property(x => x.NType).HasColumnName("nType").IsOptional();
            Property(x => x.NDisplayFlags).HasColumnName("nDisplayFlags").IsOptional();
            Property(x => x.NCompanyId).HasColumnName("nCompanyId").IsOptional();
            Property(x => x.NHalfHalf).HasColumnName("nHalfHalf").IsOptional();
            Property(x => x.WMenusectionId).HasColumnName("w_menusectionId").IsOptional();
            Property(x => x.StrWebTitle).HasColumnName("strWebTitle").IsOptional().HasMaxLength(64);
            Property(x => x.NToppingGroup1).HasColumnName("nToppingGroup1").IsOptional();
            Property(x => x.NToppingGroup2).HasColumnName("nToppingGroup2").IsOptional();
            Property(x => x.NToppingGroup3).HasColumnName("nToppingGroup3").IsOptional();
            Property(x => x.NToppingGroup4).HasColumnName("nToppingGroup4").IsOptional();
            Property(x => x.NToppingGroup5).HasColumnName("nToppingGroup5").IsOptional();
            Property(x => x.NForeColor).HasColumnName("nForeColor").IsOptional();
            Property(x => x.NBackColor).HasColumnName("nBackColor").IsOptional();
            Property(x => x.NParentId).HasColumnName("nParentId").IsOptional();
            Property(x => x.NRecipeGroup).HasColumnName("nRecipeGroup").IsOptional();
            Property(x => x.NFlags).HasColumnName("nFlags").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
