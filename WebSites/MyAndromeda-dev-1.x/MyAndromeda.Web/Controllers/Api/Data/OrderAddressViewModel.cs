using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Kendo.Mvc.Extensions;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Logging;
using MyAndromeda.Services.Orders.Events;
using MyAndromeda.Framework.Helpers;
using Kendo.Mvc.UI;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataAccess.Sites;
using MyAndromeda.Web.Controllers.Api.Data.Models;

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
        public async Task<Models.StoreOrdersSummary> GetStoreOrdersSummary([FromUri]int andromeadsiteId, [FromBody]DailyReportingQuery queryModel)
        {
            MyAndromeda.Data.Model.AndroAdmin.Store store = await this.storeDataService.Table
                .Where(e => e.AndromedaSiteId == andromeadsiteId)
                .SingleOrDefaultAsync();

            IQueryable<MyAndromeda.Data.DataWarehouse.Models.OrderHeader> ordersQuery = this.orderHeaderDataService.OrderHeaders.AsNoTracking()
                .Where(e => e.ExternalSiteID == store.ExternalId)
                .Where(e => e.TimeStamp >= queryModel.From)
                .Where(e => e.TimeStamp <= queryModel.To)
                .Include(e => e.OrderStatu);

            IQueryable<IGrouping<int, MyAndromeda.Data.DataWarehouse.Models.OrderHeader>> q = ordersQuery.GroupBy(e => e.OrderStatu.Id);

            Models.StoreOrdersSummary[] results = await q.Select(e => new Models.StoreOrdersSummary()
            {
                Cancelled = e.Count(k => k.OrderStatu.Id == 6),
                Completed = e.Count(k => k.OrderStatu.Id == 6),
                InOven = e.Count(k => k.OrderStatu.Id == 2),
                OutForDelivery = e.Count(k => k.OrderStatu.Id == 4),
                ReadyToDispatch = e.Count(k => k.OrderStatu.Id == 3),
                ReceivedByStore = e.Count(k => k.OrderStatu.Id == 1),
                Total = e.Count()
            }).ToArrayAsync();

            return results.First();
        }


        [HttpPost]
        [Route("data/{andromeadsiteId}/orders/map")]
        public async Task<IEnumerable<Models.MapResults>> Map([FromUri]int andromeadsiteId, [FromBody]DailyReportingQuery queryModel)
        {
            var results = new List<Models.MapResults>();
            try
            {
                MyAndromeda.Data.Model.AndroAdmin.Store store = await this.storeDataService.Table.Where(e => e.AndromedaSiteId == andromeadsiteId).SingleOrDefaultAsync();
                IQueryable<OrderHeader> ordersQuery = this.orderHeaderDataService.OrderHeaders
                    .AsNoTracking()
                    .Where(e => e.ExternalSiteID == store.ExternalId)
                    .Where(e => e.TimeStamp >= queryModel.From)
                    .Where(e => e.TimeStamp <= queryModel.To)
                    .Include(e => e.OrderStatu)
                    .Include(e => e.Customer);

                OrderHeader[] ordersQueryResult = await ordersQuery.ToArrayAsync();

                IEnumerable<IGrouping<Guid?, OrderHeader>> groups = ordersQueryResult.GroupBy(e => e.CustomerAddressID);

                foreach (var group in groups)
                {
                    List<OrderHeader> orders = group.ToList();
                    OrderHeader first = orders.First();

                    if (first.Customer.Address == null)
                    {
                        this.logger.Error("Customer address is null for the record: " + first.ID);
                        continue;
                    }

                    var result = new Models.MapResults()
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
                        Orders = orders.Select(k => new Models.OrderSummary()
                        {
                            FinalPrice = k.FinalPrice,
                            OrderWantedTime = k.OrderWantedTime.GetValueOrDefault()
                        }).ToList()
                    };

                    results.Add(result);
                }

            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
                throw;
            }

            return results;
        }

        [HttpPost]
        [Route("data/{andromedaSiteId}/debug-orders")]
        public async Task<DataSourceResult> GetOrders()
        {
            string body = await this.Request.Content.ReadAsStringAsync();

            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(body);
            DataQuery query = JsonConvert.DeserializeObject<DataQuery>(body);

            request.Sorts = query.Sort.Select(e => new Kendo.Mvc.SortDescriptor(e.Field, e.Dir.Equals("desc") ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending)).ToList();

            try
            {
                IQueryable<DebugOrderViewModel> ordersQuery = this.orderHeaderDataService.OrderHeaders
                .AsNoTracking()
                .Where(e => e.ExternalSiteID == this.currentSite.ExternalSiteId)
                .Include(e => e.OrderStatu)
                .Include(e => e.Customer)
                .Select(DebugOrderViewModel.FromOrder);

                DataSourceResult orders = ordersQuery.ToDataSourceResult(request); //request. //await ordersQuery.ToArrayAsync();

                return orders;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("data/debug-orders/{orderId}/orders/food")]
        public async Task<OrderFoodViewModel> GetOrderFood(Guid orderId)
        {
            var order = await this.orderHeaderDataService.OrderHeaders
                .Where(o => o.ID == orderId)
                .Select(OrderFoodViewModel.FromOrder)
                .FirstOrDefaultAsync();

            return order;
        }

        [HttpGet]
        [Route("data/debug-orders/{orderId}/orders/details")]
        public async Task<OrderDetailsViewModel> GetOrderDetails(Guid orderId)
        {
            var order = await this.orderHeaderDataService.OrderHeaders
                .Where(o => o.ID == orderId)
                .Select(OrderDetailsViewModel.FromOrder)
                .FirstOrDefaultAsync();

            return order;
        }

        [HttpGet]
        [Route("data/debug-orders/{orderId}/orders/payment")]
        public async Task<OrderPaymentViewModel> GetOrderPayment(Guid orderId)
        {
            var order = await this.orderHeaderDataService.OrderHeaders
                .Where(o => o.ID == orderId)
                .Select(OrderPaymentViewModel.FromOrder)
                .FirstOrDefaultAsync();

            return order;
        }

        [HttpGet]
        [Route("data/debug-orders/{orderId}/orders/status")]
        public async Task<OrderStatusViewModel> GetOrderStatus(Guid orderId)
        {
            var order = await this.orderHeaderDataService.OrderHeaders
                .Where(o => o.ID == orderId)
                .Select(OrderStatusViewModel.FromOrder)
                .FirstOrDefaultAsync();

            return order;
        }

        //[HttpPost]
        //[Route("data/{andromedaSiteId}/debug-orders")]
        //public async Task<DataSourceResult> List()
        //{
        //    //{"take":10,"skip":0,"page":1,"pageSize":10}
        //    string body = await this.Request.Content.ReadAsStringAsync();

        //    DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(body);
        //    DataQuery query = JsonConvert.DeserializeObject<DataQuery>(body);

        //    request.Sorts = query.Sort.Select(e => new Kendo.Mvc.SortDescriptor(e.Field, e.Dir.Equals("desc") ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending)).ToList();

        //    try
        //    {
        //        IQueryable<OrderViewModel> ordersQuery =
        //        this.orderHeaderDataService.OrderHeaders
        //        .AsNoTracking()
        //        .Where(e => e.ExternalSiteID == this.currentSite.ExternalSiteId)
        //        .Include(e => e.OrderStatu)
        //        .Include(e => e.Customer)
        //        .Select(e => new OrderViewModel()
        //        {
        //            Id = e.ID,
        //            ExternalOrderRef = e.ExternalOrderRef,
        //            TicketNumber = e.TicketNumber,
        //            CookingInstructions = e.CookingInstructions,
        //            OrderNotes = e.OrderNotes,
        //            ItemCount = e.OrderLines.Count(),
        //            DeliveryCharge = e.DeliveryCharge,
        //            Tips = e.Tips,
        //            CardCharges = e.OrderPayments.Sum(k => k.PaymentCharge),
        //            Items = e.OrderLines.Select(item => new OrderItem()
        //            {
        //                Id = item.ID,
        //                Name = item.Description,
        //                Qty = item.Qty,
        //                Price = item.Price,
        //                Person = item.Person
        //            }),
        //            PaymentLines = e.OrderPayments.Select(item => new PaymentLine()
        //            {
        //                Id = item.ID,
        //                PayTypeName = item.PayTypeName,
        //                PaymentType = item.PaymentType,
        //                Value = item.Value,
        //                Charge = item.PaymentCharge
        //            }),
        //            StatusDescription = e.OrderStatu.Description,
        //            FinalPrice = e.FinalPrice,
        //            OrderPlacedTime = e.OrderPlacedTime,
        //            OrderWantedTime = e.OrderWantedTime,
        //            Customer = new CustomerViewModel()
        //            {
        //                Id = e.Customer.ID,
        //                Name = e.Customer.FirstName,
        //                Email = e.Customer.Contacts.Where(c => c.ContactTypeId == 0).Select(c => c.Value).FirstOrDefault(),
        //                Phone = e.Customer.Contacts.Where(c => c.ContactTypeId == 1).Select(c => c.Value).FirstOrDefault(),
        //                Latitude = e.Customer.Address.Lat,
        //                Longitude = e.Customer.Address.Long,
        //                Postcode = e.Customer.Address.PostCode,
        //                Directions = e.Customer.Address.Directions
        //            },
        //            OrderAddress = new OrderAddressViewModel()
        //            {
        //                Postcode = e.CustomerAddress.ZipCode,
        //                Latitude = e.CustomerAddress.Latitude,
        //                Longitude = e.CustomerAddress.Longitude,
        //                RoadNum = e.CustomerAddress.RoadNum,
        //                RoadName = e.CustomerAddress.RoadName,
        //                Directions = e.CustomerAddress.Directions
        //            },
        //            Driver = new DriverViewModel()
        //            {
        //                Name = e.DriverName,
        //                Phone = e.DriverPhoneNumber
        //            },
        //            OrderStatusHistory = e.OrderStatusHistories.Select(s => new OrderStatus()
        //            {
        //                Id = s.Status,
        //                Description = s.OrderStatu.Description,
        //                ChangeDateTime = s.ChangedDateTime
        //            })
        //            .OrderByDescending(s => s.ChangeDateTime)
        //            .ToList(),

        //            BringgId = e.BringgTaskId,
        //            IbsOrderId = e.IbsOrders.OrderByDescending(d => d.CreatedAtUtc).Select(r => r.IbsOrderId).FirstOrDefault()
        //        });

        //        DataSourceResult orders = ordersQuery.ToDataSourceResult(request); //request. //await ordersQuery.ToArrayAsync();

        //        return orders;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }


        //    //var result = orders.ToDataSourceResult(request);

        //    //foreach (var item in result.Data) { }

        //    //return result;
        //}

        [HttpPost]
        [Route("data/{andromedaSiteId}/orders/{id}/addDriver")]
        public async Task AddDriver([FromUri]int andromedaSiteId, [FromUri]string id, [FromBody]DriverViewModel model)
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
                .Include(e => e.OrderStatu)
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


            await this.orderChangeEvents.ForEachAsync(async e =>
            {
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
                    this.notifier.Error(e.Name + " boo boo", false);
                }
            });
        }
    }

    public class OrderAddressViewModel
    {
        public string Postcode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string RoadNum { get; set; }
        public string RoadName { get; set; }
        public string Directions { get; set; }
    }

}