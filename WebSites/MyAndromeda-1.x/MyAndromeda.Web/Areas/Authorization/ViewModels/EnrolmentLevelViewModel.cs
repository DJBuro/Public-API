using MyAndromeda.Core.Authorization;
using MyAndromeda.Core.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Authorization.ViewModels
{
    public class EnrolmentLevelViewModel : IEnrolmentLevel
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}