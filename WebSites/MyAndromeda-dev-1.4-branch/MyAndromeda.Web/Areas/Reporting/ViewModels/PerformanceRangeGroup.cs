﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromedaDataAccess.Domain.Reporting.Query;

namespace MyAndromeda.Web.Areas.Reporting.ViewModels
{
    public class PerformanceRangeGroupViewModel
    {
        public string Name { get; set; }
        public FilterQuery Query { get; set; }
    }
}