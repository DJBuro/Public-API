using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Logging;
using MyAndromeda.Services.Orders.Events;
using MyAndromeda.Framework.Helpers;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

namespace MyAndromeda.Web.Controllers.Api.Data
{
    public class OrdersDataServiceController : ApiController
    {
        private readonly IMyAndromedaLogger logger;
        private readonly ICurrentSite currentSite;
        private readonly IOrderHeaderDataService orderHeaderDataService;
        private readonly IOrderChangedEvent[] orderChangeEvents;
        private readonly INotifier notifier;
        private readonly IStoreDataService storeDataService;

        public OrdersDataServiceController(ICurrentSite currentSite,
            IOrderHeaderDataService orderHeaderDataService,
            IOrderChangedEvent[] orderChangeEvents,
            IMyAndromedaLogger logger,
            INotifier notifier,
            IStoreDataService storeDataService) 
        {
            this.storeDataService = storeDataService;
            this.notifier = notifier;
            this.logger = logger;
            this.orderChangeEvents = orderChangeEvents;
            this.orderHeaderDataService = orderHeaderDataService;
            this.currentSite = currentSite;
        }

        [HttpPost]
        [Route("data/{andromeadsiteId}/orders/")]
        public async Task<Models.StoreOrdersSummary> GetStoreOrdersSummary([FromUri]int andromeadsiteId, [FromBody]MyAndromeda.Web.Controllers.Api.Data.DailyReportingDataController.Query queryModel)
        {
            var store = await this.storeDataService.Table
                .Where(e => e.AndromedaSiteId == andromeadsiteId)
                .SingleOrDefaultAsync();

            var ordersQuery = this.orderHeaderDataService.OrderHeaders.AsNoTracking()
                .Where(e => e.ExternalSiteID == store.ExternalId)
                .Where(e => e.TimeStamp >= queryModel.From)
                .Where(e => e.TimeStamp <= queryModel.To)
                .Include(e=> e.OrderStatu);

            var q = ordersQuery.GroupBy(e => e.OrderStatu.Id);
            var r = await q.Select(e => new Models.StoreOrdersSummary() { 
                Cancelled = e.Count(k=> k.OrderStatu.Id == 6),
                Completed = e.Count(k=> k.OrderStatu.Id == 6),
                InOven = e.Count(k=> k.OrderStatu.Id == 2),
                OutForDelivery = e.Count(k => k.OrderStatu.Id == 4),
                ReadyToDispatch = e.Count(k=> k.OrderStatu.Id == 3),
                ReceivedByStore = e.Count(k => k.OrderStatu.Id == 1),
                Total = e.Count()
            }).ToArrayAsync();

            return r.First();
        }


        [HttpPost]
        [Route("data/{andromeadsiteId}/orders/map")]
        public async Task<IEnumerable<Models.Bob>> Map([FromUri]int andromeadsiteId, [FromBody]MyAndromeda.Web.Controllers.Api.Data.DailyReportingDataController.Query queryModel) 
        {
            List<Models.Bob> result = new List<Models.Bob>();
            try
            {
                var store = await this.storeDataService.Table.Where(e => e.AndromedaSiteId == andromeadsiteId).SingleOrDefaultAsync();
                var ordersQuery = this.orderHeaderDataService.OrderHeaders
                    .AsNoTracking()
                    .Where(e => e.ExternalSiteID == store.ExternalId)
                    .Where(e => e.TimeStamp >= queryModel.From)
                    .Where(e => e.TimeStamp <= queryModel.To)
                    .Include(e => e.OrderStatu)
                    .Include(e => e.Customer);

                var ordersQueryResult = await ordersQuery.ToArrayAsync();

                var groups = ordersQueryResult.GroupBy(e => e.CustomerAddressID);

                foreach(var group in groups)
                {
                    var orders = group.ToList();
                    var first = orders.First();

                    if (first.Customer.Address == null) 
                    {
                        this.logger.Error("Customer address is null for the record: " + first.ID);
                        continue;
                    }

                    var bob = new Models.Bob()
                    {
                        Customer = new CustomerViewModel()
                        {
                            Id = first.Customer.ID,
                            Name = first.Customer.FirstName,
                            Email = first.Customer.Contacts.Where(c => c.ContactTypeId == 0).Select(c => c.Value).FirstOrDefault(),
                            Phone = first.Customer.Contacts.Where(c => c.ContactTypeId == 1).Select(c => c.Value).FirstOrDefault(),
                            Latitude = first.Customer.Address.Lat,
                            Longitude = first.Customer.Address.Long,
                            Postcode = first.Customer.Address.PostCode
                        },
                        OrderAddress = new OrderAddressViewModel()
                        {
                            Postcode = first.CustomerAddress.ZipCode,
                            Latitude = first.CustomerAddress.Latitude,
                            Longitude = first.CustomerAddress.Longitude
                        },
                        Orders = orders.Select(k => new Models.OrderSummary() { 
                            FinalPrice = k.FinalPrice,
                            OrderWantedTime = k.OrderWantedTime.GetValueOrDefault()
                        }).ToList()
                    };

                    result.Add(bob);
                }
                
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
                throw;
            }

            return result;
        }

        [HttpPost]
        [Route("data/{andromedaSiteId}/debug-orders")]
        public async Task<IEnumerable<OrderViewModel>> List(
            //[ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request
            ) 
        {

            try
            {
                var ordersQuery =
                this.orderHeaderDataService.OrderHeaders
                .AsNoTracking()
                .Where(e => e.ExternalSiteID == this.currentSite.ExternalSiteId)
                .Include(e => e.OrderStatu)
                .Include(e => e.Customer)
                
                .OrderByDescending(e => e.OrderPlacedTime)
                .Take(100)
                .Select(e => new OrderViewModel()
                {
                    Id = e.ID,
                    ItemCount = e.OrderLines.Count(),
                    Items = e.OrderLines.Select(item => new OrderItem() {
                        Id = item.ID,
                        Name = item.Description
                    }),
                    StatusDescription = e.OrderStatu.Description,
                    FinalPrice = e.FinalPrice,
                    OrderPlacedTime = e.OrderPlacedTime,
                    OrderWantedTime = e.OrderWantedTime,
                    Customer = new CustomerViewModel()
                    {
                        Id = e.Customer.ID,
                        Name = e.Customer.FirstName,
                        Email = e.Customer.Contacts.Where(c => c.ContactTypeId == 0).Select(c => c.Value).FirstOrDefault(),
                        Phone = e.Customer.Contacts.Where(c => c.ContactTypeId == 1).Select(c => c.Value).FirstOrDefault(),
                        Latitude = e.Customer.Address.Lat,
                        Longitude = e.Customer.Address.Long,
                        Postcode = e.Customer.Address.PostCode
                    },
                    OrderAddress = new OrderAddressViewModel()
                    {
                        Postcode = e.CustomerAddress.ZipCode,
                        Latitude = e.CustomerAddress.Latitude,
                        Longitude = e.CustomerAddress.Longitude,
                        RoadNum = e.CustomerAddress.RoadNum,
                        RoadName = e.CustomerAddress.RoadName,
                    },
                    Driver = new DriverViewModel()
                    {
                        Name = e.DriverName,
                        Phone = e.DriverPhoneNumber
                    },
                    BringgId = e.BringgTaskId,
                    IbsOrderId = e.IbsOrders.OrderByDescending(d => d.CreatedAtUtc).Select(r=> r.IbsOrderId).FirstOrDefault()
                });

                var orders = await ordersQuery.ToArrayAsync();
                return orders;
            }
            catch (Exception ex)
            {
                
                throw;
            }

            
            //var result = orders.ToDataSourceResult(request);

            //foreach (var item in result.Data) { }

            //return result;
        }

        [HttpPost]
        [Route("data/{andromedaSiteId}/orders/{id}/addDriver")]
        public async Task AddDriver([FromUri]int andromedaSiteId,[FromUri]string id, [FromBody]DriverViewModel model) 
        {
            var orderId = new Guid(id);
            var orderHeader = await this.orderHeaderDataService
                .OrderHeaders
                .Where(e => e.ID == orderId)
                .SingleOrDefaultAsync();

            orderHeader.DriverName = model.Name;
            orderHeader.DriverPhoneNumber = model.Phone;

            await this.orderHeaderDataService.SaveChangesAsync();
        }

        [HttpPost]
        [Route("data/{andromedaSiteId}/orders/{id}/updateStatus")]
        public async Task ChangeStatus([FromUri]int andromedaSiteId, [FromUri]string id, [FromBody] ChangeOrderStatusViewModel model) 
        {
            var orderId = new Guid(id);
            var orderHeader = await this.orderHeaderDataService
                .OrderHeaders
                .Include(e=> e.OrderStatu)
                .Where(e => e.ID == orderId)
                .SingleOrDefaultAsync();

            var currentStatus = orderHeader.OrderStatu;
            orderHeader.Status = model.StatusId;

            //this.orderHeaderDataService.OrderStatusHistory.Add(new MyAndromeda.Data.DataWarehouse.Models.OrderStatusHistory() {
            //    Id = Guid.NewGuid(),
            //    OrderHeaderId = orderId,
            //    Status = model.StatusId,
            //    ChangedDateTime = DateTime.UtcNow
            //});

            
            try
            {
                await this.orderHeaderDataService.SaveChangesAsync();
            }
            catch (Exception)
            {
                
                throw;
            }
            

            await this.orderChangeEvents.ForEachAsync(async e => { 
                try 
	            {
                    var r = await e.OrderStatusChangedAsync(andromedaSiteId, orderHeader, currentStatus);

                    if (r == WorkLevel.CompletedWork)
                    {
                        this.notifier.Success(e.Name + " - finished", false);
                    }
                    else 
                    {
                        this.notifier.Success(e.Name + " - no work to do", false);
                    }
                    
	            }
	            catch (Exception) 
                {
                    this.notifier.Error(e.Name + " boo boo" , false);
                }
            });
        } 
    }

    public class ChangeOrderStatusViewModel
    {
        public int StatusId { get; set; }     
    }

    public class DriverViewModel 
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class OrderViewModel 
    {
        public CustomerViewModel Customer { get; set; }
        public DriverViewModel Driver { get; set; }
        public Guid Id { get; set; }

        public int ItemCount { get; set; }
        public decimal FinalPrice { get; set; }

        public DateTime? OrderPlacedTime { get; set; }
        public DateTime? OrderWantedTime { get; set; }
        
        public int? BringgId { get; set; }
        public OrderAddressViewModel OrderAddress { get; set; }

        public IEnumerable<OrderItem> Items { get; set; }
        public string StatusDescription { get; set; }

        public long IbsOrderId { get; set; }
    }

    public class OrderItem 
    {

        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class OrderAddressViewModel 
    {
        public string Postcode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string RoadNum { get; set; }
        public string RoadName { get; set; }
    }

    public class CustomerViewModel 
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public string Longitude { get; set; }

        public string Postcode { get; set; }

        public decimal[] GeoLocation 
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(Latitude)) { return new decimal[]{ 0,0 }; }
                var a = decimal.Parse(Latitude, CultureInfo.InvariantCulture); //Convert.ToDecimal(Latitude, );
                var b = decimal.Parse(Longitude, CultureInfo.InvariantCulture); //Convert.ToDecimal(Longitude);

                return new[] { a,b };
            } 
        }
    }
}