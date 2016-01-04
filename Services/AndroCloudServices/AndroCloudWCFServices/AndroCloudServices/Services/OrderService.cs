using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using System;
using AndroCloudServices.Domain;
using AndroCloudServices.Helper;
using AndroCloudDataAccess;
using AndroCloudHelper;

namespace AndroCloudServices.Services
{
    public class OrderService
    {
        public static Response Get(
            string externalPartnerId,
            string externalSiteId,
            string externalOrderId,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            sourceId = externalPartnerId;

            // Was a externalPartnerId provided?
            if (externalPartnerId == null || externalPartnerId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingPartnerId, dataType);
            }

            // Was a externalSiteId provided?
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // externalSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Was a externalOrderId provided?
            if (externalOrderId == null || externalOrderId.Length == 0)
            {
                // externalOrderId was not provided
                return new Response(Errors.MissingOrderId, dataType);
            }

            // Check the partners details
            Guid partnerId = Guid.Empty;
            Guid orderId = Guid.Empty;
            Response response = SecurityHelper.CheckOrderGetAccess(externalPartnerId, externalOrderId, dataAccessFactory, dataType, out partnerId, out orderId);

            if (response != null)
            {
                return response;
            }

            // Get the order status
            Order order = null;
            dataAccessFactory.OrderDataAccess.GetById(orderId, out order);

            // Was there an error?
            if (order == null)
            {
                return new Response(Errors.UnknownOrderId, dataType);
            }

            // Return the order status
            return new Response(SerializeHelper.Serialize<Order>(order, dataType));
        }

        public static Response Put(
            string externalPartnerId,
            string externalSiteId,
            string data,
            DataTypeEnum dataType, 
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            sourceId = externalPartnerId;

            // Was a externalPartnerId provided?
            if (externalPartnerId == null || externalPartnerId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingPartnerId, dataType);
            }

            // Was an externalSiteId provided?
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // externalSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Check the partners details
            Guid orderId = Guid.Empty;
            Response response = SecurityHelper.CheckOrderPostAccess(externalPartnerId, externalSiteId, dataAccessFactory, dataType, out orderId);

            if (response != null)
            {
                return response;
            }

// TODO signalr implementation goes here

// TODO we must return the ExternalID from the order table in the response
            Order order = new Order();
            order.ExternalID = "123"; // Populate this so the caller can pass it back to check the order status
            order.RamesesStatusId = 1;

            // Serialize
            return new Response(SerializeHelper.Serialize<Order>(order, dataType));
        }

        public static Response Post(
            string orderData,
            string siteId,
            string internetOrderNumberParameter,
            string licenseKey,
            string hardwareKey,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory)
        {
            // Was a order provided?
            if (orderData == null || orderData.Length == 0)
            {
                // order was not provided
                return new Response(Errors.MissingOrder, dataType);
            }

            // Was a siteId provided?
            if (siteId == null || siteId.Length == 0)
            {
                // siteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Was a orderId provided?
            if (internetOrderNumberParameter == null || internetOrderNumberParameter.Length == 0)
            {
                // orderId was not provided
                return new Response(Errors.MissingOrderId, dataType);
            }

            // Is the internetOrderNumber a valid integer?
            int internetOrderNumber = 0;
            if (!int.TryParse(internetOrderNumberParameter, out internetOrderNumber))
            {
                // orderId is not an integer
                return new Response(Errors.InvalidOrderId, dataType);
            }

            // Was an licenseKey provided?
            if (licenseKey == null || licenseKey.Length == 0)
            {
                // licenseKey was not provided
                return new Response(Errors.MissingLicenseKey, dataType);
            }

            // Was an hardwareKey provided?
            if (hardwareKey == null || hardwareKey.Length == 0)
            {
                // hardwareKey was not provided
                return new Response(Errors.MissingHardwareKey, dataType);
            }

            // Extract the new status id
            // Deserialize the order
            OrderStatusUpdate orderStatusUpdate = null;
            string errorMessage = SerializeHelper.Deserialize<OrderStatusUpdate>(orderData, dataType, out orderStatusUpdate);
            if (errorMessage.Length > 0)
            {
                // There was a problem deserializing
                Response response = new Response(Errors.BadData, dataType);
                response.ResponseText = response.ResponseText.Replace("{errorMessage}", errorMessage);
                return response;
            }

            // Was a new status provided?
            if (orderStatusUpdate == null || orderStatusUpdate.Status == null || orderStatusUpdate.Status.Length == 0)
            {
                // status was not provided
                return new Response(Errors.MissingStatus, dataType);
            }

            // Was an order status provided?
            if (orderStatusUpdate.Status == null || orderStatusUpdate.Status.Length == 0)
            {
                // order status was not provided
                return new Response(Errors.MissingOrderStatusId, dataType);
            }

            // Is the orderStatusId a valid integer?
            int ramesesOrderStatusId = 0;
            if (!int.TryParse(orderStatusUpdate.Status, out ramesesOrderStatusId))
            {
                // orderId is not an integer
                return new Response(Errors.InvalidOrderStatusId, dataType);
            }

            // Check the externalSiteId is valid
            Site site = null;
            dataAccessFactory.SiteDataAccess.GetByExternalSiteId(siteId, out site);

            if (site == null)
            {
                return new Response(Errors.UnknownSiteId, dataType);
            }

            // Was the license key provided the correct license key for the site?
            if (site.LicenceKey != licenseKey)
            {
                return new Response(Errors.UnknownLicenseKey, dataType);
            }

            // Check if the orderId is valid
            Order order = null;
            dataAccessFactory.OrderDataAccess.GetByInternetOrderNumber(internetOrderNumber, out order);

            if (order == null)
            {
                return new Response(Errors.UnknownOrderId, dataType);
            }

            // Is the new order status valid?
            OrderStatus orderStatus = null;
            dataAccessFactory.OrderStatusDataAccess.GetByRamesesStatusId(ramesesOrderStatusId, out orderStatus);

            if (orderStatus == null)
            {
                // order status is not valid
                return new Response(Errors.UnkownOrderStatusId, dataType);
            }

            // Change the order status in the database
            dataAccessFactory.OrderDataAccess.Update(order.ID, orderStatus.Id);

            // Serialize
            return new Response();
        }
    }
}
