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
    // Country
    public partial class Country
    {
        public int Id { get; set; } // Id (Primary key)
        public string CountryName { get; set; } // CountryName
        public string Iso31661Alpha2 { get; set; } // ISO3166_1_alpha_2
        public int Iso31661Numeric { get; set; } // ISO3166_1_numeric
    }

}
