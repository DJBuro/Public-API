using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Andromeda.GPSIntegration;
using Andromeda.GPSIntegration.Bringg;
using MyAndromeda.Data.DataAccess.Gps;
using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Framework.Dates;
using MyAndromeda.Logging;
using MyAndromeda.Services.Bringg.Outgoing;
using MyAndromeda.Services.Gprs;
using MyAndromeda.Services.WebHooks;
using MyAndromeda.WebApiClient;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Services.Bringg
{
    public class BringgService : IBringgService
    {
        private readonly IMyAndromedaLogger logger;
        
        private readonly IStoreDataService storeDataService;
        private readonly IOrderHeaderDataService orderHeaderDataService;

        private readonly IGPSIntegrationServices gpsIntegrationServices;
        private readonly IBringgSettingsDataService gpsSettingsDataService;
        private readonly IGprsService gprsDeviceService;

        private readonly WebhookEndpointManger webhookEndpointManger;
        private readonly IWebApiClientContext webApiClientContext;
        private readonly IWorkContext workContext;
        

        public BringgService(
            WebhookEndpointManger webhookEndpointManger,
            IOrderHeaderDataService orderHeaderDataService,
            IWorkContext workContext,
            //IDateServices dateServices,
            IStoreDataService storeDataService,
            IBringgSettingsDataService gpsSettingsDataService,
            IGprsService gprsDeviceService,
            IMyAndromedaLogger logger,
            IWebApiClientContext webApiClientContext)
        {
            this.workContext = workContext;
            this.webApiClientContext = webApiClientContext;
            this.webhookEndpointManger = webhookEndpointManger;
            this.logger = logger;
            this.gprsDeviceService = gprsDeviceService;
            this.gpsSettingsDataService = gpsSettingsDataService;
            this.storeDataService = storeDataService;
            this.orderHeaderDataService = orderHeaderDataService;
            this.gpsIntegrationServices = new BringgGPSIntegrationServices();
        }
        
        public async Task<bool> IsBringgConfigured(int andromedaSiteId)
        {
            StoreGPSSetting settings = await this.gpsSettingsDataService
                .Settings
                .FirstOrDefaultAsync(e => e.Store.AndromedaSiteId == andromedaSiteId);

            if (settings == null) { return false; }

            //dynamic o = JsonConvert.DeserializeObject(settings.PartnerConfig);
            
            //if (o.isEnabled) 
            //{
            //    return true;
            //}

            return true;
        }

        public async Task<bool> ShallWeSendOrder(int andromedaSiteId, UsefulOrderStatus currentState)
        {
            if (await this.gprsDeviceService.IsStoreGprsDevicebyAndromedaSiteIdAsync(andromedaSiteId))
            {
                if (currentState == UsefulOrderStatus.OrderHasBeenCompleted) { return true; }
            }

            bool completed = currentState == UsefulOrderStatus.OrderHasBeenCompleted;
            if (completed)
                return false;

            bool cancelled = currentState == UsefulOrderStatus.OrderHasBeenCancelled;
            if (cancelled)
                return false;
    
            //and for rameses ... on any status (not completed/rejected) ... send 
            return true;
        }

        public async Task AddOrderAsync(int andromedaSiteId, Guid orderId, bool addNotes)
        {
            OrderHeader order = await this.orderHeaderDataService
                .OrderHeaders
                    .Include(e => e.Customer)
                    .Include(e => e.Customer.Address)
                    .Include(e => e.Customer.Contacts)
                    .Include(e => e.CustomerAddress)
                    .SingleOrDefaultAsync(e => e.ID == orderId);

            if (order.Customer == null)
            {
                throw new ArgumentNullException(paramName: "order.customer");
            }

            if (order.CustomerAddress == null)
            {
                throw new ArgumentNullException(paramName: "order.CustomerAddress");
            }

            Store store = await this.storeDataService.Table.SingleOrDefaultAsync(e => e.AndromedaSiteId == andromedaSiteId);
            
            Andromeda.GPSIntegration.Model.Customer customer = this.CreateCustomer(order.Customer);
            Andromeda.GPSIntegration.Model.Address customerAddress = this.CreateAddress(order.CustomerAddress);
            Andromeda.GPSIntegration.Model.Order bringgOrder = this.CreateOrder(store, order, customerAddress);
            
            bool success = false;

            ResultEnum result = ResultEnum.UnknownError;

            Action<string, DebugLevel> log = (message, level) => {
                if (level == DebugLevel.Notify)
                {
                    this.logger.Debug(message);
                }
                if (level == DebugLevel.Error)
                {
                    this.logger.Error(message);
                }
            };

            result = this.gpsIntegrationServices.CustomerPlacedOrder(store.Id, customer, bringgOrder, addNotes, log);

            if (result == ResultEnum.OK)
            {
                success = true; 
            }

            //try again
            if(!success)
            {                    
                this.logger.Error(message: "Sending Bringg task failed with the phone number... switch to email only");
                
                customer.Phone = null;

                result = this.gpsIntegrationServices.CustomerPlacedOrder(store.Id, customer, bringgOrder, addNotes, log);
            }

            if (result == ResultEnum.OK)
            {
                success = true;
            }

            if (!success) 
            {
                this.logger.Error(message: "Failed with phone and email");
            }

            switch (result)
            {
                case ResultEnum.Disabled:
                    {
                        throw new Exception(message: "Disabled?");
                    }
                case ResultEnum.NoStoreSettings:
                    {
                        throw new Exception(message: "No Store Settings");
                    }
                case ResultEnum.UnknownError:
                    {
                        throw new Exception(message: "Unknown Error... cool. (Bringg had a boo boo but wont be specific)");
                    }
                case ResultEnum.OK:
                    {
                        int id;
                        if (int.TryParse(bringgOrder.BringgTaskId, out id))
                        {
                            order.BringgTaskId = Convert.ToInt32(bringgOrder.BringgTaskId);
                            await this.orderHeaderDataService.SaveChangesAsync();

                            await this.AnnounceNewBringgTask(andromedaSiteId, order, customer);
                            
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        

        private async Task AnnounceNewBringgTask(int andromedaSiteId, OrderHeader order, Andromeda.GPSIntegration.Model.Customer customer) 
        {
            try
            {
                var outgoingMessage = new OutgoingWebHookBringg() 
                {
                    AndromedaSiteId = andromedaSiteId,
                    ExternalSiteId = order.ExternalSiteID,
                    AndromedaOrderId = order.ID.ToString(),
                    ExternalId = order.ExternalOrderRef,
                    Id = order.BringgTaskId.GetValueOrDefault(),
                    Source = "Create Bringg Task", 
                    Status = "0",
                    AndromedaOrderStatusId = order.Status,
                    UserId = customer.PartnerId
                };

                //todo : make sendWebHooksService transient so i don't have to talk via a webservice
                //await this.sendWebHooksService.CallEndpoints(outgoingMessage, e => e.BringUpdates);

                try
                {
                    this.logger.Debug("Calling: " + webApiClientContext.BaseAddress + this.webhookEndpointManger.BringgEndpoint);
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(webApiClientContext.BaseAddress);

                        HttpResponseMessage response = await client.PostAsJsonAsync(this.webhookEndpointManger.BringgEndpoint, outgoingMessage);

                        if (!response.IsSuccessStatusCode)
                        {
                            string message = string.Format(format: "Could not call : {0}", arg0: this.webhookEndpointManger.BringgEndpoint);
                            string responseMessage = await response.Content.ReadAsStringAsync();

                            throw new WebException(message, new Exception(responseMessage));
                        }
                    }
                }
                catch (Exception e)
                {
                    this.logger.Error(e);
                }

            }
            catch (Exception e)
            {
                this.logger.Error("I couldn't notify myself about bringg being created for some reason. :)");
                this.logger.Error(e);
            }   
        }

        public async Task<UpdateDriverResult> UpdateDriverAsync(
            int andromedaSiteId,
            Guid? internalOrderId,
            string externalOrderId)
        {
            IQueryable<OrderHeader> query = this.orderHeaderDataService.OrderHeaders;

            query = internalOrderId.HasValue
                ? query.Where(e => e.ID == internalOrderId.Value)
                : query.Where(e => e.ExternalOrderRef == externalOrderId);

            var order = await
                query
                    .Select(e => new
                    {
                        Id = e.ID,
                        ExternalOrderId = e.ExternalOrderRef,
                        e.DriverName,
                        e.DriverPhoneNumber,
                        e.ExternalOrderRef,
                        e.Bags,
                        e.BringgTaskId
                    })
                    .SingleOrDefaultAsync();

            if (order == null)
            {
                return Bringg.UpdateDriverResult.CantFindOrderInWarehouse;
            }
            if (!order.BringgTaskId.HasValue)
            {
                return Bringg.UpdateDriverResult.NoBringgTaskId;
            }
            if (string.IsNullOrWhiteSpace(order.DriverName))
            {
                return Bringg.UpdateDriverResult.NoDriverName;
            }
            if (string.IsNullOrWhiteSpace(order.DriverPhoneNumber))
            {                                                            
                return Bringg.UpdateDriverResult.NoDriverPhoneNumber;
            }

            Andromeda.GPSIntegration.Model.Driver driver = this.CreateDriver(order.DriverName, order.DriverPhoneNumber);

            ResultEnum result = this.gpsIntegrationServices
                .AssignDriverToOrder(
                    andromedaSiteId,
                    order.BringgTaskId.ToString(),
                    order.Bags,
                    driver, 
                    (message, level) => {
                        if (level == DebugLevel.Notify) { this.logger.Debug(message); }
                        if (level == DebugLevel.Error) { this.logger.Error(message); }
                    }
                );

            if (result == ResultEnum.UnknownError) { return Bringg.UpdateDriverResult.UnknownError; }

            return Bringg.UpdateDriverResult.Success;
        }

        public async Task<bool> CancelOrder(int andromedaSiteId, Guid? internalOrderId, string externalOrderId)
        {
            IQueryable<OrderHeader> query = this.orderHeaderDataService.OrderHeaders;
            IQueryable<Store> storeQuery = this.storeDataService.Table;

            query = internalOrderId.HasValue
                ? query.Where(e => e.ID == internalOrderId.Value)
                : query.Where(e => e.ExternalOrderRef == externalOrderId);

            var order = await
                query
                    .Select(e => new
                    {
                        Id = e.ID,
                        ExternalOrderId = e.ExternalOrderRef,

                        e.BringgTaskId
                    })
                    .SingleOrDefaultAsync();

            if (!order.BringgTaskId.HasValue) { return true; }

            var storeId = await storeQuery
                .Where(e => e.AndromedaSiteId == andromedaSiteId)
                .Select(e => e.Id)
                .SingleOrDefaultAsync();


            var result = this.gpsIntegrationServices.CancelOrder(storeId, order.BringgTaskId.ToString());

            switch (result)
            {
                case ResultEnum.Disabled: return true;
                case ResultEnum.NoStoreSettings: return true;
                case ResultEnum.UnknownError: return false;
                case ResultEnum.OK: return true;
            }

            return true;
        }

        private Andromeda.GPSIntegration.Model.Order CreateOrder(
            Store store,
            OrderHeader orderHeader,
            Andromeda.GPSIntegration.Model.Address address)
        {
            var model = new Andromeda.GPSIntegration.Model.Order();

            if (orderHeader.BringgTaskId.HasValue)
            {
                model.BringgTaskId = orderHeader.BringgTaskId.Value.ToString();
            }

            model.AndromedaOrderId = //"bob";
                orderHeader.TicketNumber.GetValueOrDefault()
                + " - " + orderHeader.Customer.FirstName;

            model.Address = address;

            //LocalizationContext localizationContext = LocalizationContext.Create(store.UiCulture, store.TimeZoneInfoId);
            //IDateServices dateService = DateServicesFactory.CreateInstance(localizationContext);

            //DateTime utcScheduledForDateTime = dateService.ConvertToUtcFromLocal(orderHeader.OrderWantedTime).GetValueOrDefault();
            //TimeZoneInfo.ConvertTimeToUtc(orderHeader.OrderWantedTime.GetValueOrDefault(), this.TimeZone);
            //string utcScheduledForString = utcScheduledForDateTime.ToString(format: "yyyy-MM-ddTHH:mm:ss.fffZ");


            model.ScheduledAt = orderHeader.OrderWantedTime.GetValueOrDefault(); // utcScheduledForDateTime;
            model.TotalPrice = orderHeader.FinalPrice;

            if (orderHeader.paytype == "CARD")
            {
                model.HasBeenPaid = true;
            }

            model.Note = string.Empty;

            if (orderHeader.TicketNumber.GetValueOrDefault() > 0)
            {
                model.Note += "Ticket Number: " + orderHeader.TicketNumber.Value + "; ";
            }

            if (orderHeader.Bags.HasValue)
            {
                model.Note += "Number of Bags: " + orderHeader.Bags.Value.ToString() + "; ";
            }

            model.Note += "Directions: ";
            
            bool hasCustomerDirections = orderHeader.Customer.Address != null && !string.IsNullOrWhiteSpace(orderHeader.Customer.Address.Directions);
            bool hasOrderDirections = orderHeader.CustomerAddress != null && !string.IsNullOrWhiteSpace(orderHeader.CustomerAddress.Directions);

            if (hasOrderDirections || hasCustomerDirections)
            {
                model.Note += hasOrderDirections
                    ? orderHeader.CustomerAddress.Directions
                    : orderHeader.Customer.Address.Directions;
                model.Note += ";";
            }
            else 
            {
                model.Note += "none;"; 
            }

            if (!string.IsNullOrWhiteSpace(orderHeader.CookingInstructions)) 
            {
                model.Note += " Cooking instructions: " + orderHeader.CookingInstructions;
            }

            return model;
        }

        private Andromeda.GPSIntegration.Model.Customer CreateCustomer(MyAndromeda.Data.DataWarehouse.Models.Customer customer)
        {
            var email = customer.Contacts
                                .Where(e => e.ContactTypeId == 0)
                                .Select(e => e.Value)
                                .FirstOrDefault();

            var phone = customer.Contacts
                                .Where(e => e.ContactTypeId == 1)
                                .Select(e => e.Value)
                                .FirstOrDefault();

            return new Andromeda.GPSIntegration.Model.Customer()
            {
                Id = customer.ID,
                Email = string.IsNullOrWhiteSpace(email) ? string.Empty : email,
                Phone = phone,
                Name = customer.FirstName,
                Lat = null,
                Lng = null,
                PartnerId = string.Empty
            };
        }

        private Andromeda.GPSIntegration.Model.Address CreateAddress(MyAndromeda.Data.DataWarehouse.Models.CustomerAddress address)
        {
            var model = new Andromeda.GPSIntegration.Model.Address()
            {
                Country = "UK",
                County = address.State,
                //Directions = string.Empty,
                Lat = null,
                Long = null,
                Locality = string.Empty,
                Org1 = null,
                Org2 = null,
                Org3 = null,
                Postcode = address.ZipCode,
                Prem1 = null,
                Prem2 = null,
                Prem3 = null,
                Prem4 = null,
                Prem5 = null,
                Prem6 = null,
                RoadName = address.RoadName,
                RoadNum = address.RoadNum,
                State = string.Empty,
                Town = address.City
            };

            return model;
        }

        private Andromeda.GPSIntegration.Model.Driver CreateDriver(string name, string phone)
        {
            return new Andromeda.GPSIntegration.Model.Driver()
            {
                Name = name,
                Phone = phone
            };
        }
    }
}