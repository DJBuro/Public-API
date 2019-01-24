namespace AndroCloudWCFServices
{
    using System.Collections.Generic;
    using System.IO;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using AndroCloudServices.Models;

    [ServiceContract]
    public interface IRESTServicesV2
    {
        /* Host */
        [OperationContract]
        Stream GetHost(string applicationId);

        [OperationContract]
        Stream GetMenu(string siteId, string applicationId, string version);

        [OperationContract]
        Stream GetMenuImages(string siteId, string applicationId);

        /* Sites */
        [OperationContract]
        Stream GetSites(string groupIdFilter, string maxDistanceFilter, string longitudeFilter, string latitudeFilter, string deliveryZoneFilter, string applicationId);

        [OperationContract]
        Stream GetSite(string siteId, string applicationId, int gotMenuVersion, string statusCheck);

        [OperationContract]
        Stream GetSiteDetails(string siteId, string applicationId);
        
        [OperationContract]
        Stream GetDeliveryZones(string siteId, string applicationId);

        [OperationContract]
        Stream GetDeliveryTowns(string applicationId);

        [OperationContract]
        Stream GetDeliveryRoads(string applicationId, string postcode);

        /* Orders */
        [OperationContract]
        Task<Stream> PutOrder(Stream input, string siteId, string orderId, string applicationId);

        [OperationContract]
        Stream GetOrder(string siteId, string orderId, string applicationId);

        [OperationContract]
        Stream CheckOrderVouchers(Stream input, string siteId, string applicationId);

        /* Customer accounts */
        [OperationContract]
        Stream GetCustomer(string username, string applicationId, string externalSiteId);

        [OperationContract]
        Stream GetCustomerOrders(string username, string applicationId);

        [OperationContract]
        Stream GetCustomerOrder(string username, string orderid, string applicationId);

        [OperationContract]
        Stream PutCustomer(Stream input, string username, string applicationId, string externalSiteId);

        [OperationContract]
        Stream PostCustomer(Stream input, string username, string applicationId, string newPassword);

        [OperationContract]
        Stream PutPasswordResetRequest(string username, string applicationId);

        [OperationContract]
        Stream PostPasswordResetRequest(Stream input, string username, string applicationId);

        [OperationContract]
        Stream PostCustomerLoyalty(Stream input, string username, string applicationId);

        [OperationContract]
        Stream PostApplyCustomerLoyalty(Stream input, string username, string action, string externalOrderRef, string applicationId);

        /* Feedback */
        [OperationContract]
        Stream PutFeedback(Stream input, string applicationId);

        [OperationContract]
        Stream SitePutFeedback(Stream input, string siteId, string applicationId);

        /* Telemetry */
        [OperationContract]
        Stream PutTelemetrySession(Stream input, string siteId, string applicationId);
        [OperationContract]
        Stream PutTelemetry(Stream input, string siteId, string applicationId);

        [OperationContract]
        Stream GetSite3(string siteId, string applicationId, int gotMenuVersion, string statusCheck);


        /// <summary>
        /// New call for AW 2.0. Retrieves all sites for an application with most of their details
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [OperationContract(Name = "SitesWithDetails")]
        IEnumerable<AcsSiteDetails> GetSites(string applicationId);
    }
}