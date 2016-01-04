using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Andromeda.WebOrdering
{
    [ServiceContract]
    public interface IRESTServices
    {
        [OperationContract]
        void TEST();

        [OperationContract]
        Stream SiteList(string deliveryZoneFilter, string key);

        [OperationContract]
        Stream SiteDetails(string siteId, string key);

        [OperationContract]
        Stream Menu(string siteId, string key);

        [OperationContract]
        Stream Order(Stream stream, string siteId, string key);

        [OperationContract]
        Stream CheckOrderVouchers(Stream stream, string siteId, string key);

        [OperationContract]
        Stream MercuryPayment(Stream stream, string siteId, string key);

        [OperationContract]
        Stream DataCashPayment(Stream stream, string siteId, string key);

        [OperationContract]
        Stream MercanetPayment(Stream stream, string siteId, string key);

        [OperationContract]
        Stream DeliveryZones(string siteId, string key);

        [OperationContract]
        Stream GetCustomers(string username, string siteId, string key);

        [OperationContract]
        Stream PutCustomers(Stream stream, string username, string siteId, string key);

        [OperationContract]
        Stream PostCustomers(Stream stream, string username, string key, string newPassword);

        [OperationContract]
        Stream PutPasswordResetRequest(Stream stream, string username, string key);

        [OperationContract]
        Stream PostPasswordResetRequest(Stream stream, string username, string key, string passwordResetToken);

        [OperationContract]
        Stream MercanetCallback(Stream stream);

        [OperationContract]
        Stream GetSite(string siteId, string key, int gotMenuVersion, string statusCheck);

        [OperationContract]
        Stream GetDeliveryTowns(string key);

        [OperationContract]
        Stream GetDeliveryRoads(string postcode, string key);

        [OperationContract]
        Stream GetCustomerOrders(string username, string key);

        [OperationContract]
        Stream GetCustomerOrder(string username, string orderId, string key);

        [OperationContract]
        Stream PayPalPayment(Stream stream, string siteId, string key);

        [OperationContract]
        Stream PayPalCallback(Stream stream, string orderId, string key);

        [OperationContract]
        Stream AddUpdateWebSite(Stream stream, string key);

        [OperationContract]
        Stream Feedback(Stream stream, string key);

        [OperationContract]
        Stream SiteFeedback(Stream stream, string siteId, string key);

        [OperationContract]
        Stream TelemetrySession(Stream stream, string siteId, string key);

        [OperationContract]
        Stream Telemetry(Stream stream, string siteId, string key);
    }
}
