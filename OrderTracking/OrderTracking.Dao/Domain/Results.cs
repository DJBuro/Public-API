using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderTracking.Dao.Domain
{
    public class Results
    {
        public List<string> ErrorMessages { get; set; }
        public List<string> WarningMessages { get; set; }
        public bool Success { get; set; }
    }
}
