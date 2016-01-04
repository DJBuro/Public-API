using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;

namespace MyAndromeda.Data.AcsServices.Context
{
    public interface IActiveMenuContext : IDependency
    {
        SiteMenuMediaServer MediaServer { get; set; }
        SiteMenu Menu { get; set; }

        string ContentPath { get; }

        void Setup(int andromedaSiteId, string externalSiteId);
    }
}
