using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess;
using AndroCloudDataAccess.Domain;
using AndroCloudServices.Domain;
using AndroCloudHelper;

namespace AndroCloudServices.Helper
{
    public class SecurityHelper
    {
        public static Response CheckMenuGetAccess(string externalPartnerId, string externalSiteId, IDataAccessFactory dataAccessFactory, DataTypeEnum dataType, out Guid siteId)
        {
            siteId = Guid.Empty;

            // Check the externalPartnerId is valid
            Partner partner = null;
            dataAccessFactory.PartnerDataAccess.Get(externalPartnerId, out partner);

            if (partner == null)
            {
                return new Response(Errors.UnknownPartnerId, dataType);
            }

            // Check the externalSiteId is valid
            Site site = null;
            dataAccessFactory.SiteDataAccess.GetByExternalSiteId(externalSiteId, out site);

            if (site == null)
            {
                return new Response(Errors.UnknownSiteId, dataType);
            }

            // Is the partner allowed to access the site?
            dataAccessFactory.SiteDataAccess.GetByIdAndPartner(partner.Id, site.Id, out site);

            if (site == null)
            {
                return new Response(Errors.PartnerAccessToSiteDenied, dataType);
            }

            siteId = site.Id;

            return null;
        }

        public static Response CheckMenuPostAccess(string externalSiteId, string licenseKey, IDataAccessFactory dataAccessFactory, DataTypeEnum dataType, out Guid siteId)
        {
            siteId = Guid.Empty;

            // Check the externalSiteId is valid
            Site site = null;
            dataAccessFactory.SiteDataAccess.GetByExternalSiteId(externalSiteId, out site);

            if (site == null)
            {
                return new Response(Errors.UnknownSiteId, dataType);
            }

            if (licenseKey != site.LicenceKey)
            {
                return new Response(Errors.InvalidLicenceKey, dataType);
            }

            siteId = site.Id;

            return null;
        }

        public static Response CheckSitesGetAccess(
            string externalPartnerId, 
            string externalGroupId, 
            IDataAccessFactory dataAccessFactory, 
            DataTypeEnum dataType,
            out Guid partnerId,
            out Guid? groupId)
        {
            partnerId = Guid.Empty;
            groupId = null;

            // Check the externalPartnerId is valid
            Partner partner = null;
            dataAccessFactory.PartnerDataAccess.Get(externalPartnerId, out partner);

            if (partner == null)
            {
                return new Response(Errors.UnknownPartnerId, dataType);
            }

            partnerId = partner.Id;

            // If there's a externalGroupId check that the partner is allowed to access the group
            if (externalGroupId != null && externalGroupId.Length > 0)
            {
                // Does the group exist?
                Group group = null;
                dataAccessFactory.GroupDataAccess.Get(partner.Id, externalGroupId, out group);

                if (group == null)
                {
                    return new Response(Errors.UnknownGroupId, dataType);
                }

                groupId = group.Id;
            }
            
            return null;
        }

        public static Response CheckSiteDetailsGetAccess(
            string externalPartnerId,
            string externalSiteId,
            IDataAccessFactory dataAccessFactory,
            DataTypeEnum dataType,
            out Guid partnerId,
            out Guid siteId)
        {
            partnerId = Guid.Empty;
            siteId = Guid.Empty;

            // Check the externalPartnerId is valid
            Partner partner = null;
            dataAccessFactory.PartnerDataAccess.Get(externalPartnerId, out partner);

            if (partner == null)
            {
                return new Response(Errors.UnknownPartnerId, dataType);
            }

            // Check the externalSiteId is valid
            Site site = null;
            dataAccessFactory.SiteDataAccess.GetByExternalSiteId(externalSiteId, out site);

            if (site == null)
            {
                return new Response(Errors.UnknownSiteId, dataType);
            }

            // Is the partner allowed to access the site?
            dataAccessFactory.SiteDataAccess.GetByIdAndPartner(partner.Id, site.Id, out site);

            if (site == null)
            {
                return new Response(Errors.PartnerAccessToSiteDenied, dataType);
            }

            siteId = site.Id;

            return null;
        }

        public static Response CheckOrderPostAccess(
            string externalPartnerId,
            string externalSiteId,
            IDataAccessFactory dataAccessFactory,
            DataTypeEnum dataType,
            out Guid siteId)
        {
            siteId = Guid.Empty;

            // Check the externalPartnerId is valid
            Partner partner = null;
            dataAccessFactory.PartnerDataAccess.Get(externalPartnerId, out partner);

            if (partner == null)
            {
                return new Response(Errors.UnknownPartnerId, dataType);
            }

            // Check the externalSiteId is valid
            Site site = null;
            dataAccessFactory.SiteDataAccess.GetByExternalSiteId(externalSiteId, out site);

            if (site == null)
            {
                return new Response(Errors.UnknownSiteId, dataType);
            }

            // Is the partner allowed to send ordersto the site?
            dataAccessFactory.SiteDataAccess.GetByIdAndPartner(partner.Id, site.Id, out site);

            if (site == null)
            {
                return new Response(Errors.PartnerAccessToSiteDenied, dataType);
            }

            return null;
        }

        public static Response CheckOrderGetAccess(
            string externalPartnerId,
            string externalOrderId,
            IDataAccessFactory dataAccessFactory,
            DataTypeEnum dataType,
            out Guid partnerId,
            out Guid orderId)
        {
            partnerId = Guid.Empty;
            orderId = Guid.Empty;

            // Check the externalPartnerId is valid
            Partner partner = null;
            dataAccessFactory.PartnerDataAccess.Get(externalPartnerId, out partner);

            if (partner == null)
            {
                return new Response(Errors.UnknownPartnerId, dataType);
            }

            partnerId = partner.Id;

            // Check the externalPartnerId is valid
            Order order = null;
            dataAccessFactory.OrderDataAccess.GetByExternalOrderNumber(externalOrderId, out order);

            if (order == null)
            {
                return new Response(Errors.UnknownOrderId, dataType);
            }

            orderId = order.ID;

            // Check that the partner is allowed to access the site
            order = null;
            dataAccessFactory.OrderDataAccess.GetByPartnerIdOrderId(partnerId, orderId, out order);

            if (order == null)
            {
                return new Response(Errors.PartnerAccessToSiteDenied, dataType);
            }

            return null;
        }
    }
}
