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
    // OTD_vw
    internal partial class OtdVwConfiguration : EntityTypeConfiguration<OtdVw>
    {
        public OtdVwConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".OTD_vw");
            HasKey(x => x.Nstoreid);

            Property(x => x.Nstoreid).HasColumnName("nstoreid").IsRequired();
            Property(x => x.Thedate).HasColumnName("thedate").IsOptional();
            Property(x => x.AvgSecs).HasColumnName("avg_secs").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
