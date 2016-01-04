using Andromeda.WebOrdering.Model;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Diagnostics;

namespace Andromeda.WebOrdering.Services
{
    public interface IStaticPageGenerator
    {
        Site[] GetSiteList(string domainName);
        StoreMenu GetSiteMenu(string domainName, WebSiteServicesData webSiteServicesData, string siteId);
    }
}