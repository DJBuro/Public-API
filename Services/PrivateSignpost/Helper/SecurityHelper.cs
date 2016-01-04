using AndroCloudHelper;
using SignpostDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateSignpost.Helper
{
    public class SecurityHelper
    {
        //public static Response CheckHostsGetAccess(
        //   int andromedaSiteId,
        //   string licenseKey,
        //   ISignpostDataAccess signpostDataAccess,
        //   DataTypeEnum dataType,
        //   out Guid siteId)
        //{
        //    siteId = Guid.Empty;

        //    // Check the andromedaSiteId is valid
        //    AndroCloudDataAccess.Domain.Site site = null;
        //    dataAccessFactory.SiteDataAccess.GetByAndromedaSiteIdAndLive(andromedaSiteId, out site);

        //    if (site == null)
        //    {
        //        return new Response(Errors.UnknownSiteId, dataType);
        //    }

        //    if (licenseKey != site.LicenceKey)
        //    {
        //        return new Response(Errors.InvalidLicenceKey, dataType);
        //    }

        //    siteId = site.Id;

        //    return null;
        //}
    }
}