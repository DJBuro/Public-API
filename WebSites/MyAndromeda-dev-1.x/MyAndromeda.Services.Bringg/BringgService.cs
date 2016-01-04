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
using MyAndromeda.Services.Gprs;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

namespace MyAndromeda.Services.Bringg
{
    public class BringgService : IBringgService
    {
        private readonly IDateServices dateServices;
        private readonly IStoreDataService storeDataService;
        private readonly IOrderHeaderDataService orderHeaderDataService;

        private readonly IGPSIntegrationServices gpsIntegrationServices;
        private readonly IBringgSettingsDataService gpsSettingsDataService;
        private readonly IGprsService gprsDeviceService;

        public BringgService(IOrderHeaderDataService orderHeaderDataService,
            IDateServices dateServices,
            IStoreDataService storeDataService,
            IBringgSettingsDataService gpsSettingsDataService,
            IGprsService gprsDeviceService)
        {
            this.gprsDeviceService = gprsDeviceService;
            this.gpsSettingsDataService = gpsSettingsDataService;
            this.storeDataService = storeDataService;
            this.orderHeaderDataService = orderHeaderDataService;
            this.dateServices = dateServices;
            this.gpsIntegrationServices = new BringgGPSIntegrationServices();
        }

        public async Task<bool> IsBringgConfigured(int andromedaSiteId)
        {
            var any = await this.gpsSettingsDataService
                .Settings
                .AnyAsync(e => e.Store.AndromedaSiteId == andromedaSiteId);

            return any;
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
            //return 
            //    UsefulOrderStatus.OrderHasBeenReceivedByTheStore;
        }

        public async Task AddOrderAsync(int andromedaSiteId, Guid orderId)
        {
            var order = await this.orderHeaderDataService
                .OrderHeaders
                    .Include(e => e.Customer)
                    .Include(e => e.Customer.Address)
                    .Include(e => e.Customer.Contacts)
                    .Include(e => e.CustomerAddress)
                    .SingleOrDefaultAsync(e => e.ID == orderId);

            if (order.Customer == null)
            {
                throw new ArgumentNullException("order.customer");
            }

            if (order.CustomerAddress == null)
            {
                throw new ArgumentNullException("order.CustomerAddress");
            }

            var customer = this.CreateCustomer(order.Customer);
            var customerAddress = this.CreateAddress(order.CustomerAddress);

            var bringgOrder = this.CreateOrder(order, customerAddress);

            var store = await this.storeDataService.Table.SingleOrDefaultAsync(e => e.AndromedaSiteId == andromedaSiteId);
            var result = this.gpsIntegrationServices.CustomerPlacedOrder(store.Id, customer, bringgOrder);


            switch (result)
            {
                case ResultEnum.Disabled:
                    {
                        throw new Exception("Disabled?");
                    }
                case ResultEnum.NoStoreSettings:
                    {
                        throw new Exception("No Store Settings");
                    }
                case ResultEnum.UnknownError:
                    {
                        throw new Exception("Unknown Error... cool. (Bringg had a boo boo but wont be specific)");
                    }
                case ResultEnum.OK:
                    {
                        int id;
                        if (int.TryParse(bringgOrder.BringgTaskId, out id))
                        {
                            order.BringgTaskId = Convert.ToInt32(bringgOrder.BringgTaskId);
                            await this.orderHeaderDataService.SaveChangesAsync();
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
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

            var driver = this.CreateDriver(order.DriverName, order.DriverPhoneNumber);

            var result = this.gpsIntegrationServices
                .AssignDriverToOrder(
                    andromedaSiteId,
                    order.BringgTaskId.ToString(),
                    order.Bags,
                    driver
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
            OrderHeader orderHeader,
            Andromeda.GPSIntegration.Model.Address address)
        {
            var model = new Andromeda.GPSIntegration.Model.Order();

            //model.AndromedaOrderId = (orderHeader.TicketNumber.HasValue ? 
            //    orderHeader.TicketNumber.Value.ToString() : orderHeader.ExternalOrderRef) 
            //    + " - " + orderHeader.SiteName ;
            //orderHeader.ID.ToString();

            if (orderHeader.BringgTaskId.HasValue)
            {
                model.BringgTaskId = orderHeader.BringgTaskId.Value.ToString();
            }

            model.AndromedaOrderId = //"bob";
                orderHeader.TicketNumber.GetValueOrDefault()
                + " - " + orderHeader.Customer.FirstName;

            model.Address = address;
            model.ScheduledAt = orderHeader.OrderWantedTime.GetValueOrDefault();
            model.TotalPrice = orderHeader.FinalPrice;

            if (orderHeader.paytype == "CARD")
            {
                model.HasBeenPaid = true;
            }

            model.Note = string.Empty;

            if (orderHeader.TicketNumber.GetValueOrDefault() > 0)
            {
                model.Note += "Ticket Number: " + orderHeader.TicketNumber.Value;
            }

            if (orderHeader.Bags.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(model.Note))
                {
                    model.Note += "; ";
                }

                model.Note += "Number of Bags: " + orderHeader.Bags.Value.ToString();
            }

            if (orderHeader.Customer.Address != null) 
            { 
                if (string.IsNullOrWhiteSpace(orderHeader.Customer.Address.Directions)) 
                {
                    model.Note += "; ";
                    model.Note += orderHeader.Customer.Address.Directions;
                }
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