﻿using MyAndromeda.Core.Authorization;
using MyAndromeda.Core.Site;
using MyAndromedaDataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Authorization.ViewModels
{
    public class StoreEnrolmentViewModel
    {
        public int StoreId { get; set; }
        public Site Site { get; set; }

        public IEnumerable<IEnrolmentLevel> Options { get; set; }
        public int SelectedOptionId { get; set; }
        
        public IEnumerable<IPermission> CurrentPermissions { get; set; }
    }
}