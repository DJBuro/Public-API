namespace AndroCloudServices.Helper
{
    using System;
    using AndroCloudDataAccess.Domain;
    using AndroCloudHelper;
    using DataWarehouseDataAccess.Domain;
    using Site = AndroCloudDataAccess.Domain.Site;

    internal class SecurityHelper
    {

        public static Response CheckMenuGetAccess(
            string externalApplicationId, 
            string externalSiteId,
            AndroCloudDataAccess.IDataAccessFactory dataAccessFactory, 
            DataTypeEnum dataType, 
            out Guid siteId)
        {
            siteId = Guid.Empty;

            // Check the externalApplicationId is valid
            ACSApplication acsApplication = null;
            dataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out acsApplication);

            if (acsApplication == null)
            {
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            // Check the externalSiteId is valid
            AndroCloudDataAccess.Domain.Site site = null;
            dataAccessFactory.SiteDataAccess.GetByExternalSiteId(externalSiteId, out site);

            if (site == null)
            {
                return new Response(Errors.UnknownSiteId, dataType);
            }

            // Is the partner allowed to access the site?
            dataAccessFactory.SiteDataAccess.GetByIdAndApplication(acsApplication.Id, site.Id, out site);

            if (site == null)
            {
                return new Response(Errors.ApplicationAccessToSiteDenied, dataType);
            }

            siteId = site.Id;

            return null;
        }

        public static Response CheckHostsGetAccess(
            string externalApplicationId,
            AndroCloudDataAccess.IDataAccessFactory dataAccessFactory,
            DataTypeEnum dataType)
        {
            // Check the externalApplicationId is valid
            ACSApplication acsApplication = null;
            dataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out acsApplication);

            if (acsApplication == null)
            {
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            return null;
        }

        public static Response CheckHostsGetAccess(
            int andromedaSiteId, 
            string licenseKey,
            AndroCloudDataAccess.IDataAccessFactory dataAccessFactory,
            DataTypeEnum dataType, 
            out Guid siteId)
        {
            siteId = Guid.Empty;

            // Check the andromedaSiteId is valid
            AndroCloudDataAccess.Domain.Site site = null;
            dataAccessFactory.SiteDataAccess.GetByAndromedaSiteIdAndLive(andromedaSiteId, out site);

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

        public static Response CheckMenuPostAccess(
            int andromedaSiteId, 
            string licenseKey,
            AndroCloudDataAccess.IDataAccessFactory dataAccessFactory, 
            DataTypeEnum dataType, 
            out Guid siteId)
        {
            siteId = Guid.Empty;

            // Check the andromedaSiteId is valid
            AndroCloudDataAccess.Domain.Site site = null;
            dataAccessFactory.SiteDataAccess.GetByAndromedaSiteIdAndLive(andromedaSiteId, out site);

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
            string externalApplicationId,
            AndroCloudDataAccess.IDataAccessFactory dataAccessFactory, 
            DataTypeEnum dataType,
            out int? applicationId)
        {
            applicationId = null;

            // Check the externalApplicationId is valid
            ACSApplication acsApplication = null;
            dataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out acsApplication);

            if (acsApplication == null)
            {
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            applicationId = acsApplication.Id;
            
            return null;
        }

        public static Response CheckSiteDetailsGetAccess(
            string externalApplicationId,
            string externalSiteId,
            AndroCloudDataAccess.IDataAccessFactory dataAccessFactory,
            DataTypeEnum dataType,
            out int? applicationId,
            out Guid siteId)
        {
            applicationId = null;
            siteId = Guid.Empty;

            // Check the externalApplicationId is valid
            dataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out ACSApplication acsApplication);

            if (acsApplication == null)
            {
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            applicationId = acsApplication.Id;

            // Check the externalSiteId is valid
            dataAccessFactory.SiteDataAccess.GetByExternalSiteId(externalSiteId, out Site site);

            if (site == null)
            {
                return new Response(Errors.UnknownSiteId, dataType);
            }

            // Is the application allowed to access the site?
            dataAccessFactory.SiteDataAccess.GetByIdAndApplication(applicationId.Value, site.Id, out site);

            if (site == null)
            {
                return new Response(Errors.ApplicationAccessToSiteDenied, dataType);
            }

            siteId = site.Id;

            return null;
        }

        public static Response CheckOrderPostAccess(
            string externalApplicationId,
            string externalSiteId,
            AndroCloudDataAccess.IDataAccessFactory dataAccessFactory,
            DataTypeEnum dataType,
            out int applicationId,
            out int andromedaSiteId)
        {
            applicationId = -1;
            andromedaSiteId = -1;

            // Check the externalApplicationId is valid
            ACSApplication acsApplication = null;
            dataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out acsApplication);

            if (acsApplication == null)
            {
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            // Check the externalSiteId is valid
            AndroCloudDataAccess.Domain.Site site = null;
            dataAccessFactory.SiteDataAccess.GetByExternalSiteId(externalSiteId, out site);

            if (site == null)
            {
                return new Response(Errors.UnknownSiteId, dataType);
            }

            // Is the partner allowed to send orders to the site?
            dataAccessFactory.SiteDataAccess.GetByIdAndApplication(acsApplication.Id, site.Id, out site);

            if (site == null)
            {
                return new Response(Errors.ApplicationAccessToSiteDenied, dataType);
            }

            // Return the application id
            if (site != null)
            {
                applicationId = acsApplication.Id;
                andromedaSiteId = site.AndroId;
            }

            return null;
        }

        //public static Response CheckOrderGetAccess(
        //    string externalApplicationId,
        //    string externalOrderId,
        //    AndroCloudDataAccess.IDataAccessFactory dataAccessFactory,
        //    DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory,
        //    DataTypeEnum dataType,
        //    out int? applicationId,
        //    out Guid orderId)
        //{
        //    applicationId = null;
        //    orderId = Guid.Empty;

        //    // Check the externalApplicationId is valid
        //    ACSApplication application = null;
        //    dataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out application);

        //    if (application == null)
        //    {
        //        return new Response(Errors.UnknownApplicationId, dataType);
        //    }

        //    applicationId = application.Id;

        //    // Check the externalOrderId is valid
        //    AndroCloudDataAccess.Domain.Order order = null;
        //    dataWarehouseDataAccessFactory.OrderDataAccess.GetByExternalOrderNumber(externalOrderId, out order);

        //    if (order == null)
        //    {
        //        return new Response(Errors.UnknownOrderId, dataType);
        //    }

        //    // We've got the application - we need to return the id
        //    orderId = order.ID;

        //    // Check that the application is allowed to access the site
        //    order = null;
        //    dataWarehouseDataAccessFactory.OrderDataAccess.GetByApplicationIdOrderId(applicationId.Value, orderId, out order);

        //    if (order == null)
        //    {
        //        return new Response(Errors.ApplicationAccessToSiteDenied, dataType);
        //    }

        //    return null;
        //}

        public static Response CheckCustomerOrderGetAccess(
            string externalApplicationId,
            string username, 
            string password,
            AndroCloudDataAccess.IDataAccessFactory androCloudDataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory,
            DataTypeEnum dataType, 
            out int? applicationId,
            out Guid? customerId)
        {
            applicationId = null;
            customerId = null;

            // Check the externalApplicationId is valid
            ACSApplication application = null;
            androCloudDataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out application);

            if (application == null)
            {
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            applicationId = application.Id;

            // Check the username is valid and the customer is associated with the application
            Customer customer = null;
            dataWarehouseDataAccessFactory.CustomerDataAccess.GetByUsernamePassword(username, password, application.Id, out customer);

            if (customer == null)
            {
                return new Response(Errors.UnknownCustomerId, dataType);
            }

            // We've got the application - we need to return the id
            customerId = customer.Id;

            return null;
        }
    }
}
