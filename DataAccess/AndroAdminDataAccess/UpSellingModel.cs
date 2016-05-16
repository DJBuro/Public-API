using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AndroAdminDataAccess.Domain.WebOrderingSetup
{

    public class UpSellingModel {
        public bool Enabled { get; set; }
        public List<UpSellingDisplayCategory> DisplayCategories { get; set; }
    }

}