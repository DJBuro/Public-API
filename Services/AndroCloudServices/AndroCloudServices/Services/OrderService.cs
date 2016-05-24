using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using System;
using AndroCloudServices.Domain;
using AndroCloudServices.Helper;
using AndroCloudDataAccess;
using DataWarehouseDataAccess;
using AndroCloudHelper;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using DataWarehouseDataAccess.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace AndroCloudServices.Services
{
    public class OrderService
    {
        public static Response Get(
            string externalApplicationId,
            string externalSiteId,
            string externalOrderId,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory dataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory,
            out string sourceId)
        {
            sourceId = externalApplicationId;

            // Was a externalPartnerId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was a externalSiteId provided?
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // externalSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Was an external order if provided?
            if (externalOrderId == null || externalOrderId.Length == 0)
            {
                // externalOrderId was not provided
                return new Response(Errors.MissingOrderId, dataType);
            }

            DataWarehouseDataAccess.Domain.Order order = null;
            Response response = null;

            if (response == null)
            {
                // For testing / staging we can bypass SignalR for specific stores
                response = TestHelper.CheckForStoreSignalRBypass(externalSiteId, externalOrderId, dataType);
            }

            if (response == null)
            {
                // Check the application details
                int applicationId = -1;
                int siteId = -1;
                response = SecurityHelper.CheckOrderPostAccess(externalApplicationId, externalSiteId, dataAccessFactory, dataType, out applicationId, out siteId);

                if (response != null)
                {
                    return response;
                }

                // Get the order status
                dataWarehouseDataAccessFactory.OrderDataAccess.GetByExternalIdApplicationId(externalOrderId, applicationId, out order);

                // Was there an error?
                if (order == null)
                {
                    return new Response(Errors.UnknownOrderId, dataType);
                }
            }

            // Return the order status
            return new Response(SerializeHelper.Serialize<DataWarehouseDataAccess.Domain.Order>(order, dataType));
        }

        public static async Task<Response> Put(
            string externalApplicationId,
            string externalSiteId,
            string externalOrderId,
            string data,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory dataAccessFactory)
        {
            // Was a externalPartnerId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was an externalSiteId provided?
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // externalSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Was an orderId provided?
            if (externalOrderId == null || externalOrderId.Length == 0)
            {
                // externalOrderId was not provided
                return new Response(Errors.MissingOrderId, dataType);
            }

            // Check the partners details
            int applicationId = -1;
            int andromedaSiteId = -1;
            Response response = SecurityHelper.CheckOrderPostAccess(externalApplicationId, externalSiteId, dataAccessFactory, dataType, out applicationId, out andromedaSiteId);

            if (response == null)
            {
                // For testing / staging we can bypass SignalR for specific stores
                response = TestHelper.CheckForStoreSignalRBypass(externalSiteId, dataType);

                if (response != null) return response;
            }

            // Get the hosts
            List<PrivateHostV2> hostList = null;
            dataAccessFactory.HostDataAccess.GetBestPrivateV2(andromedaSiteId, applicationId, out hostList);

            // Fix urls for debugging
            HostService.CheckForDevOverride<PrivateHostV2>(hostList);

            // Was a host returned?
            if ((hostList == null || hostList.Count == 0))
            {
                ErrorHelper.Log.Error("GetBestPrivateV2 returned no hubs for andromedaSiteId: " + andromedaSiteId + " applicationId: " + applicationId);
                return new Response(Errors.InternalError, dataType);
            }

            string url = "";
            foreach (PrivateHostV2 host in hostList)
            {
                if (host.Type == "PrivateHubWebService")
                {
                    url = host.Url;
                    break;
                }
            }

            if (url.Length == 0)
            {
                ErrorHelper.Log.Error("GetBestPrivateV2 returned no PrivateHubWebService hubs for andromedaSiteId: " + andromedaSiteId + " applicationId: " + applicationId);
                response = new Response(Errors.InternalError, dataType);
            }

            if (response == null)
            {
                url += externalSiteId + "/" + externalOrderId + "?applicationId=" + applicationId; // externalApplicationId;

                response = await PutAsync(
                    data,
                    dataType,
                    url);
            }

            return response;
        }

        public static async Task<Response> PutRameses(
            string andromedaSiteIdParameter,
            string ramsesesOrderNumberString,
            string orderData,
            string licenseKey,
            string hardwareKey,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory dataAccessFactory)
        {
            int andromedaSiteId = 0;

            // Was an andromedaSiteId provided?
            if (andromedaSiteIdParameter == null || andromedaSiteIdParameter.Length == 0)
            {
                // andromedaSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Is andromedaSiteId an integer?
            if (!int.TryParse(andromedaSiteIdParameter, out andromedaSiteId))
            {
                // andromedaSiteId is not an integer
                return new Response(Errors.InvalidSiteId, dataType);
            }

            // Was a order provided?
            if (orderData == null || orderData.Length == 0)
            {
                // Order was not provided
                return new Response(Errors.MissingOrder, dataType);
            }

            // Was a internetOrderNumber provided?
            if (ramsesesOrderNumberString == null || ramsesesOrderNumberString.Length == 0)
            {
                // internetOrderNumber was not provided
                return new Response(Errors.MissingOrderId, dataType);
            }

            // Is the ramesesOrderNumber a valid integer?
            int ramesesOrderNumber = 0;
            if (!int.TryParse(ramsesesOrderNumberString, out ramesesOrderNumber))
            {
                // ramesesOrderNumber is not an integer
                return new Response(Errors.InvalidOrderId, dataType);
            }

            // Was a licenseKey provided?
            if (licenseKey == null || licenseKey.Length == 0)
            {
                // licenseKey was not provided
                return new Response(Errors.MissingLicenseKey, dataType);
            }

            // Was a hardwareKey provided?
            if (hardwareKey == null || hardwareKey.Length == 0)
            {
                // hardwareKey was not provided
                return new Response(Errors.MissingHardwareKey, dataType);
            }

            // Check the andromedaSiteId is valid
            AndroCloudDataAccess.Domain.Site site = null;
            dataAccessFactory.SiteDataAccess.GetByAndromedaSiteIdAndLive(andromedaSiteId, out site);

            if (site == null)
            {
                return new Response(Errors.UnknownSiteId, dataType);
            }

            // Was the license key provided the correct license key for the site?
            if (site.LicenceKey != licenseKey)
            {
                return new Response(Errors.UnknownLicenseKey, dataType);
            }

            string url = ConfigurationManager.AppSettings["privateHubWebServiceUrl"];

            if (url.Length == 0)
            {
                ErrorHelper.Log.Error("Missing web.config setting privateHubWebServiceUrl");
                return new Response(Errors.InternalError, dataType);
            }

            url += site.AndroId + "/" + ramsesesOrderNumberString;

            return await PutAsync(
                orderData,
                dataType,
                url);
        }

        private static async Task<Response> PutAsync(
            string orderData,
            DataTypeEnum dataType,
            string url)
        {
            Response response = null;

            RestCallResult restCallResult = null;

            string httpDataType = dataType == DataTypeEnum.XML ? "Application/XML" : "Application/JSON";

            restCallResult = await HttpHelper.RestCall(
                "PUT",
                url,
                httpDataType,
                httpDataType,
                null,
                orderData);

            if (restCallResult.Success && restCallResult.HttpStatus == HttpStatusCode.OK)
            {
                ACSHubResult acsHubResult = null;
                string error = AndroCloudHelper.SerializeHelper.Deserialize<ACSHubResult>(restCallResult.ResponseData, dataType, out acsHubResult);

                if (error.Length == 0)
                {
                    // Web service response successfully decoded
                    if (acsHubResult.Code == -1)
                    {
                        // The acs hub web service returned an error
                        response = new Response(Errors.InternalError, dataType);

                        ErrorHelper.Log.Error("ACS hub error: " + acsHubResult.Message + " " + acsHubResult.Payload);
                    }
                    else if (acsHubResult.Code == 0 || acsHubResult.Code == 2700)
                    {
                        // We must return the orderId in the response
                        DataWarehouseDataAccess.Domain.Order order = new DataWarehouseDataAccess.Domain.Order()
                        {
                            // Return this so that the caller (iPhone app or whatever) can pass it back when they want to check the order status
                            StoreOrderId = acsHubResult.Payload,
                            // Initally the order status will be "Order taken"
                            RamesesStatusId = 1,
                            // Has the order actually been placed or is it just on its way to the store?
                            IsProvisional = (acsHubResult.Code == 2700 ? true : false)
                        };

                        // Serialize
                        response = new Response(SerializeHelper.Serialize<DataWarehouseDataAccess.Domain.Order>(order, dataType));
                    }
                    else
                    {
                        // Failure!!
                        response = new Response(new Error(acsHubResult.Message + " " + acsHubResult.Payload, (int)acsHubResult.Code, ResultEnum.BadRequest), dataType);
                    }
                }
                else
                {
                    // Error deserializing data returned by the acs hub web service
                    response = new Response(Errors.InternalError, dataType);

                    ErrorHelper.Log.Error("ACS hub response deserialize failed with: " + error + " data: " + restCallResult.ResponseData);
                }
            }
            else
            {
                // Error calling the acs hub web service
                response = new Response(Errors.InternalError, dataType);

                ErrorHelper.Log.Error("ACS hub web service call failed with: " + restCallResult.HttpStatus.ToString() + " response: " + restCallResult.ResponseData == null ? "null" : restCallResult.ResponseData);
            }

            return response;
        }

        public static Response Post(
            string orderData,
            string andromedaSiteIdParameter,
            string ramsesesOrderNumberString,
            string licenseKey,
            string hardwareKey,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory dataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory,
            out OrderStatusUpdate orderStatusUpdate,
            out AndroCloudDataAccess.Domain.Site site)
        {
            orderStatusUpdate = null;
            site = null;

            int andromedaSiteId = 0;

            // Was an andromedaSiteId provided?
            if (andromedaSiteIdParameter == null || andromedaSiteIdParameter.Length == 0)
            {
                // andromedaSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Is andromedaSiteId an integer?
            if (!int.TryParse(andromedaSiteIdParameter, out andromedaSiteId))
            {
                // andromedaSiteId is not an integer
                return new Response(Errors.InvalidSiteId, dataType);
            }

            // Was a order provided?
            if (orderData == null || orderData.Length == 0)
            {
                // Order was not provided
                return new Response(Errors.MissingOrder, dataType);
            }

            // Was a internetOrderNumber provided?
            if (ramsesesOrderNumberString == null || ramsesesOrderNumberString.Length == 0)
            {
                // internetOrderNumber was not provided
                return new Response(Errors.MissingOrderId, dataType);
            }

            // Is the ramesesOrderNumber a valid integer?
            int ramesesOrderNumber = 0;
            if (!int.TryParse(ramsesesOrderNumberString, out ramesesOrderNumber))
            {
                // ramesesOrderNumber is not an integer
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
            //OrderStatusUpdate orderStatusUpdate = null;
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

            // Check the andromedaSiteId is valid
            site = null;
            dataAccessFactory.SiteDataAccess.GetByAndromedaSiteIdAndLive(andromedaSiteId, out site);

            if (site == null)
            {
                return new Response(Errors.UnknownSiteId, dataType);
            }

            // Was the license key provided the correct license key for the site?
            if (site.LicenceKey != licenseKey)
            {
                return new Response(Errors.UnknownLicenseKey, dataType);
            }

            // Update the order status
            string errorText = dataWarehouseDataAccessFactory.OrderDataAccess.UpdateOrderStatus
            (
                ramesesOrderNumber, 
                site.ExternalId, 
                ramesesOrderStatusId,
                orderStatusUpdate.Driver,
                orderStatusUpdate.DriverId,
                orderStatusUpdate.DriverMobileNumber,
                orderStatusUpdate.TicketNumber,
                orderStatusUpdate.Bags
            );

            if (errorText != null && errorText.Length > 0)
            {
                if (errorText.StartsWith("Unknown orderId"))
                {
                    return new Response(Errors.UnknownOrderId, dataType);
                }
                else
                {
                    return new Response(Errors.InternalError, dataType);
                }
            }

            // Serialize
            return new Response();
        }

        public static Response GetCustomerOrders(
            string externalApplicationId,
            string username, 
            string password, 
            DataTypeEnum dataType, 
            AndroCloudDataAccess.IDataAccessFactory androCloudDataAccessFactory, 
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory,
            out string sourceId)
        {
            sourceId = externalApplicationId;

            // Was an externalApplicationId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was a username provided?
            if (username == null || username.Length == 0)
            {
                // username was not provided
                return new Response(Errors.MissingUsername, dataType);
            }

            // Was a password provided?
            if (password == null || password.Length == 0)
            {
                // password was not provided
                return new Response(Errors.MissingPassword, dataType);
            }

            Response response = null;
            CutomerOrders customerOrders = null;
            if (response == null)
            {
                // Check the application details
                int? applicationId = null;
                Guid? customerId = null;

                Guid orderId = Guid.Empty;
                response = SecurityHelper.CheckCustomerOrderGetAccess(
                    externalApplicationId, 
                    username, 
                    password, 
                    androCloudDataAccessFactory, 
                    dataWarehouseDataAccessFactory,
                    dataType, 
                    out applicationId,
                    out customerId);

                if (response != null)
                {
                    return response;
                }

                // Get the customers orders
                List<DataWarehouseDataAccess.Domain.OrderHeader> orderHeaders = null;
                dataWarehouseDataAccessFactory.OrderDataAccess.GetOrderHeadersByApplicationIdCustomerId(customerId, applicationId.Value, out orderHeaders);

                // Was there an error?
                if (orderHeaders == null)
                {
                    return new Response(Errors.UnknownOrderId, dataType);
                }

                customerOrders = new CutomerOrders();
                foreach (DataWarehouseDataAccess.Domain.OrderHeader orderHeader in orderHeaders)
                {
                    customerOrders.Add(new CustomerOrder() { Id = orderHeader.Id.ToString(), ForDateTime = orderHeader.ForDateTime, OrderStatus = orderHeader.Status });
                }
            }

            // Return the customers orders
            return new Response(SerializeHelper.Serialize<CutomerOrders>(customerOrders, dataType));
        }

        public static Response GetCustomerOrder(
            string externalApplicationId,
            string externalOrderId,
            string username,
            string password,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory androCloudDataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory,
            out string sourceId)
        {
            sourceId = externalApplicationId;

            // Was an externalApplicationId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was an order id provided?
            if (externalOrderId == null || externalOrderId.Length == 0)
            {
                // externalOrderId was not provided
                return new Response(Errors.MissingOrderId, dataType);
            }

            // Was a username provided?
            if (username == null || username.Length == 0)
            {
                // username was not provided
                return new Response(Errors.MissingUsername, dataType);
            }

            // Was a password provided?
            if (password == null || password.Length == 0)
            {
                // password was not provided
                return new Response(Errors.MissingPassword, dataType);
            }

            Response response = null;
            CustomerOrderDetails customerOrderDetails = null;
            if (response == null)
            {
                // Check the application details
                int? applicationId = null;
                Guid? customerId = null;

                Guid orderId = Guid.Empty;
                response = SecurityHelper.CheckCustomerOrderGetAccess(
                    externalApplicationId,
                    username,
                    password,
                    androCloudDataAccessFactory,
                    dataWarehouseDataAccessFactory,
                    dataType,
                    out applicationId,
                    out customerId);

                if (response != null)
                {
                    return response;
                }

                OrderDetails orderDetails = null;
                dataWarehouseDataAccessFactory.OrderDataAccess.GetOrderByOrderIdApplicationIdCustomerId(externalOrderId, customerId, applicationId.Value, out orderDetails);
                
                customerOrderDetails = new CustomerOrderDetails()
                {
                    Id = orderDetails.ExternalOrderRef,
                    ForDateTime = orderDetails.ForDateTime,
                    OrderStatus = orderDetails.OrderStatus,
                    OrderTotal = orderDetails.OrderTotal,
                    DeliveryCharge = orderDetails.DeliveryCharge,
                    PaymentCharge = orderDetails.PaymentCharge,
                    OrderLines = new CustomerOrderLines(),
                    Deals = new CustomerOrderDeals(),
                    Discounts = new CustomerOrderDiscounts()
                };

                // Menu items
                foreach (OrderLine orderLine in orderDetails.OrderLines)
                {
                    CustomerOrderLine customerOrderLine = new CustomerOrderLine()
                    {
                        ChefNotes = orderLine.ChefNotes,
                        MenuId = orderLine.MenuId,
                        Person = orderLine.Person,
                        ProductName = orderLine.ProductName,
                        Quantity = orderLine.Quantity,
                        UnitPrice = orderLine.UnitPrice,
                        Modifiers = new CustomerOrderLineModifiers(),
                        Cat1 = orderLine.Cat1,
                        Cat2 = orderLine.Cat2
                    };

                    foreach (Modifier modifier in orderLine.Modifiers)
                    {
                        customerOrderLine.Modifiers.Add
                        (
                            new CustomerOrderLineModifier()
                            {
                                Description = modifier.Description,
                                Price = modifier.Price,
                                Quantity = modifier.Quantity,
                                Removed = modifier.Removed,
                                Id = modifier.Id
                            }
                        );
                    }

                    // Add the order line
                    customerOrderDetails.OrderLines.Add(customerOrderLine);
                }

                // Deals
                foreach (OrderLine dealOrderLine in orderDetails.Deals)
                {
                    CustomerOrderDeal customerOrderDeal = new CustomerOrderDeal()
                    {
                        DealLines = new List<CustomerDealLine>(),
                        MenuId = dealOrderLine.MenuId,
                        ProductName = dealOrderLine.ProductName,
                        UnitPrice = dealOrderLine.UnitPrice
                    };

                    foreach (OrderLine dealLineOrderLine in dealOrderLine.ChildOrderLines)
                    {
                        CustomerDealLine customerDealLine =
                            new CustomerDealLine()
                            {
                                ChefNotes = dealLineOrderLine.ChefNotes,
                                MenuId = dealLineOrderLine.MenuId,
                                Modifiers = new CustomerOrderLineModifiers(),
                                Person = dealLineOrderLine.Person,
                                ProductName = dealLineOrderLine.ProductName,
                                Quantity = dealLineOrderLine.Quantity,
                                UnitPrice = dealLineOrderLine.UnitPrice
                            };

                        customerOrderDeal.DealLines.Add(customerDealLine);

                        foreach (Modifier modifier in dealLineOrderLine.Modifiers)
                        {
                            customerDealLine.Modifiers.Add
                            (
                                new CustomerOrderLineModifier()
                                {
                                    Description = modifier.Description,
                                    Price = modifier.Price,
                                    Quantity = modifier.Quantity,
                                    Removed = modifier.Removed,
                                    Id = modifier.Id
                                }
                            );
                        }
                    }

                    // Add the deal
                    customerOrderDetails.Deals.Add(customerOrderDeal);
                }

                // Discounts
                foreach (OrderDiscount orderDiscount in orderDetails.Discounts)
                {
                    customerOrderDetails.Discounts.Add
                    (
                        new CustomerOrderDiscount()
                        {
                            Amount = orderDiscount.Amount,
                            Reason = orderDiscount.Reason,
                            Type = orderDiscount.Type
                        }
                    );
                }

                // TESTING !!!
                //customerOrderDetails = new CustomerOrderDetails()
                //{
                //    Id = "1",
                //    ForDateTime = DateTime.UtcNow,
                //    OrderStatus = 1,
                //    OrderTotal = 8.69m,
                //    OrderLines = new CustomerOrderLines()
                //};

                //customerOrderDetails.OrderLines.Add(new CustomerOrderLine() { MenuId = 261, ProductName = "tartare salmon and tuna", Quantity = 1, UnitPrice = 3.79m, ChefNotes = "No nuts", Person = "Bob" });
                //customerOrderDetails.OrderLines.Add(new CustomerOrderLine() { MenuId = 48, ProductName = "white chocolate yoghurt & seasonal fruit", Quantity = 1, UnitPrice = 3.25m, ChefNotes = "", Person = "" });
                //customerOrderDetails.OrderLines.Add(new CustomerOrderLine() { MenuId = 222, ProductName = "vitsu water", Quantity = 1, UnitPrice = 1.65m, ChefNotes = "", Person = "" });

// !!!! IMPORTANT - MAKE SURE QUERY INCLUDES APPID, ORDERID AND USERID - SO YOU CAN'T GET OTHER PEOPLES ORDERS!!!
                // Get the customers orders
                //               androCloudDataAccessFactory.OrderDataAccess.GetByExternalIdApplicationIdCustomerId(externalOrderId, externalApplicationId, out order);

                // Was there an error?
                //if (order == null)
                //{
                //    return new Response(Errors.UnknownOrderId, dataType);
                //}
            }

            // Return the customers orders
            return new Response(SerializeHelper.Serialize<CustomerOrderDetails>(customerOrderDetails, dataType));
        }

        public static Response CheckOrderVouchers(
            string externalApplicationId,
            string externalSiteId,
            string data,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory dataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory)
        {
            // Was a externalPartnerId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was an externalSiteId provided?
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // externalSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            

            // Check the partners details
            int applicationId = -1;
            int andromedaSiteId = -1;
            Response response = SecurityHelper.CheckOrderPostAccess(externalApplicationId, externalSiteId, dataAccessFactory, dataType, out applicationId, out andromedaSiteId);
            
            // CheckOrderPostAccess returns NULL when there is no error 
            if (response != null) return response;

            // Deserialize the order
            AndroCloudServices.Domain.Order order = null;
            string errorMessage = SerializeHelper.Deserialize<AndroCloudServices.Domain.Order>(data, dataType, out order);

            if ((errorMessage == null || errorMessage.Length == 0) && order != null)
            {
                // Validate voucher-codes
                if (order.Vouchers != null && order.Vouchers.Count > 0)
                {
                    
                    ValidateVoucherCodes(order, andromedaSiteId, dataWarehouseDataAccessFactory);
                }
            }

            // Return the list of vouchers only (not the full order details)
            return new Response(SerializeHelper.Serialize<Vouchers>(order.Vouchers, dataType));
        }

        /// <summary>
        /// Validate Vouchers
        /// </summary>
        /// <param name="order"></param>
        /// <param name="andromedaSiteId"></param>
        /// <param name="dataWarehouseDataAccessFactory"></param>
        private static void ValidateVoucherCodes
            (AndroCloudServices.Domain.Order order, 
            int andromedaSiteId, 
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory)
        {
            foreach (Voucher voucher in order.Vouchers)
            {
                voucher.IsValid = false;
                voucher.ErrorCode = Errors.NoError.ErrorCode;
                voucher.EffectType = string.Empty;
                voucher.EffectValue = 0;
            }

            if (IsMoreThanOneVoucher(order))
                return;
            
            foreach (Voucher voucher in order.Vouchers)
            {
                VoucherCode voucherInDb;
                dataWarehouseDataAccessFactory.VoucherDataAccess.GetSingleVoucherByAndromedaSiteId(andromedaSiteId, voucher.VoucherCode, out voucherInDb);
                
                if (IsUnknown(voucherInDb, voucher))
                    continue;
                if (IsNotActive(voucherInDb, voucher))
                    continue;
                if (IsInvalidOccassion(voucherInDb, voucher, order))
                    continue;
                if (IsInvalidMinimumOrderAmount(voucherInDb, voucher, order))
                    continue;
                if (IsInvalidDate(voucherInDb, voucher, order))
                    continue;
                if (IsInvalidDayOfWeek(voucherInDb, voucher, order))
                    continue;
                if (IsInvalidTime(voucherInDb, voucher, order))
                    continue;
                if (IsMaxRepetitionsExceeded(voucherInDb, voucher, order, dataWarehouseDataAccessFactory))
                    continue;
                
                voucher.IsValid = true;
                voucher.EffectType = voucherInDb.DiscountType;
                voucher.EffectValue = voucherInDb.DiscountValue;
            }
        }

        /// <summary>
        /// If more than one voucher is sent with the order then return the same error for them all (3001)
        /// </summary>
        /// <param name="voucherInDb"></param>
        /// <param name="voucher"></param>
        /// <returns></returns>
        private static bool IsMoreThanOneVoucher(Domain.Order order)
        {
            if (order.Vouchers.Count > 1)
            {
                foreach (Voucher voucher in order.Vouchers)
                {
                    voucher.ErrorCode = Errors.Voucher_MaxOneVoucher.ErrorCode;
                }
            }
            return order.Vouchers.Count > 1;
        }

        /// <summary>
        /// Check if voucher code does not exists in database (error 3000).
        /// </summary>
        /// <param name="voucherInDb"></param>
        /// <param name="voucher"></param>
        /// <returns></returns>
        private static bool IsUnknown(VoucherCode voucherInDb, Voucher voucher)
        {
            if (voucherInDb == null)
            {
                voucher.ErrorCode = Errors.Voucher_UnKnown.ErrorCode;
            }
            return voucher.ErrorCode > 0;
        }

        /// <summary>
        /// Check if voucher code is enabled in the database (3002).
        /// </summary>
        /// <param name="voucherInDb"></param>
        /// <param name="voucher"></param>
        /// <returns></returns>
        private static bool IsNotActive(VoucherCode voucherInDb, Voucher voucher)
        {
            if (!voucherInDb.IsActive || voucherInDb.IsRemoved)
            {
                voucher.ErrorCode = Errors.Voucher_InActive.ErrorCode;
            }
            return voucher.ErrorCode > 0;
        }

        /// <summary>
        /// Check the occasion type (3003).
        /// </summary>
        /// <param name="voucherInDb"></param>
        /// <param name="voucher"></param>
        /// <param name="Domain.Order"></param>
        /// <returns></returns>
        private static bool IsInvalidOccassion(VoucherCode voucherInDb, Voucher voucher, Domain.Order order)
        {
            string dbVoucherOccassions = voucherInDb.Occasions != null ? string.Join(",", voucherInDb.Occasions).ToLower() : string.Empty;
            if (!dbVoucherOccassions.Contains(order.Type.ToLower()))
            {
                voucher.ErrorCode = Errors.Voucher_InvalidOccasion.ErrorCode;
            }
            return voucher.ErrorCode > 0;
        }

        /// <summary>
        /// Order amount not in range with Voucher minumum order amount(3004).
        /// </summary>
        /// <param name="voucherInDb"></param>
        /// <param name="voucher"></param>
        /// <param name="Domain.Order"></param>
        /// <returns></returns>
        private static bool IsInvalidMinimumOrderAmount(VoucherCode voucherInDb, Voucher voucher, Domain.Order order)
        {
            if (voucherInDb.MinimumOrderAmount != null && ((order.Pricing.priceAfterDiscount / 100) < voucherInDb.MinimumOrderAmount))
            {
                voucher.ErrorCode = Errors.Voucher_MinimumAmountNotMet.ErrorCode;
            }
            return voucher.ErrorCode > 0;
        }

        /// <summary>
        /// order wanted Date falls within the voucher availability (3005)
        /// </summary>
        /// <param name="voucherInDb"></param>
        /// <param name="voucher"></param>
        /// <param name="Domain.Order"></param>
        /// <returns></returns>
        private static bool IsInvalidDate(VoucherCode voucherInDb, Voucher voucher, Domain.Order order)
        {
            if (voucherInDb.StartDateTime == null || voucherInDb.EndDataTime == null)
                return false;
            DateTime startDateTime = voucherInDb.StartDateTime ?? DateTime.MinValue;
            DateTime endDateTime = voucherInDb.EndDataTime ?? DateTime.MinValue;
            if (!(order.OrderWantedTime.Ticks >= startDateTime.Ticks && order.OrderWantedTime.Ticks <= endDateTime.Ticks))
            {
                voucher.ErrorCode = Errors.Voucher_InvalidDate.ErrorCode;
            }
            return voucher.ErrorCode > 0;
        }

        /// <summary>
        /// order wanted day falls within the voucher availability ex: Monday, Tuesday etc., (3006).
        /// </summary>
        /// <param name="voucherInDb"></param>
        /// <param name="voucher"></param>
        /// <param name="Domain.Order"></param>
        /// <returns></returns>
        private static bool IsInvalidDayOfWeek(VoucherCode voucherInDb, Voucher voucher, Domain.Order order)
        {
            bool skipAvailabilityValidation = string.IsNullOrEmpty(string.Join(",", voucherInDb.AvailableOnDays));
            string dbAvailableDays = voucherInDb.AvailableOnDays != null ? string.Join(",", voucherInDb.AvailableOnDays).ToLower() : string.Empty;
            if (!skipAvailabilityValidation && !dbAvailableDays.Equals("ALL", StringComparison.OrdinalIgnoreCase) &&
                    !dbAvailableDays.Contains(order.OrderWantedTime.DayOfWeek.ToString().ToLower()))
            {
                voucher.ErrorCode = Errors.Voucher_InvalidDayOfWeek.ErrorCode;
            }
            return voucher.ErrorCode > 0;
        }

        /// <summary>
        /// order wanted Time falls within the voucher availability (3007)
        /// </summary>
        /// <param name="voucherInDb"></param>
        /// <param name="voucher"></param>
        /// <param name="Domain.Order"></param>
        /// <returns></returns>
        private static bool IsInvalidTime(VoucherCode voucherInDb, Voucher voucher, Domain.Order order)
        {
            bool skipTimeValidations = (voucherInDb.StartTimeOfDayAvailable == null || voucherInDb.EndTimeOfDayAvailable == null);
            TimeSpan startTime = voucherInDb.StartTimeOfDayAvailable ?? TimeSpan.FromSeconds(0);
            TimeSpan endTime = voucherInDb.EndTimeOfDayAvailable ?? TimeSpan.FromSeconds(0);

            if (!skipTimeValidations)
            {
                if (!(order.OrderWantedTime.TimeOfDay.Ticks >= startTime.Ticks && order.OrderWantedTime.TimeOfDay.Ticks <= endTime.Ticks))
                {
                    voucher.ErrorCode = Errors.Voucher_InvalidTimeOfDay.ErrorCode;
                }
            }
            return voucher.ErrorCode > 0;
        }

        /// <summary>
        /// Max repetitions limit exceeded (3008)
        /// </summary>
        /// <param name="voucherInDb"></param>
        /// <param name="voucher"></param>
        /// <param name="Domain.Order"></param>
        /// <returns></returns>
        private static bool IsMaxRepetitionsExceeded(VoucherCode voucherInDb, 
            Voucher voucher, 
            Domain.Order order, 
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory)
        {
            if (voucherInDb.MaxRepetitions !=null)
            {
                int maxRepetitions = voucherInDb.MaxRepetitions ?? 0;
                // Was Customer ID available
                if (string.IsNullOrEmpty(order.Customer.CustomerId))
                {
                    // Customer ID was not provided
                    voucher.ErrorCode = Errors.Voucher_CustomerId.ErrorCode;
                }
                else
                {
                    int usedVoucherCount;
                    dataWarehouseDataAccessFactory.VoucherDataAccess.GetUsedVoucherCount
                        (voucherInDb.Id.ToString(), order.Customer.CustomerId, out usedVoucherCount);

                    if (usedVoucherCount >= maxRepetitions)
                    {
                        voucher.ErrorCode = Errors.Voucher_ExceededCustomerRepetitions.ErrorCode;
                    }
                }
            }
            return voucher.ErrorCode > 0;
        }
    }
}
