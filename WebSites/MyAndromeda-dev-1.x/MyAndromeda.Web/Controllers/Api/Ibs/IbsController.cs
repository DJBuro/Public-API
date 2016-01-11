using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Http;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Services.Ibs;
using MyAndromeda.Services.Ibs.Acs;
using MyAndromeda.Services.Ibs.Models;
using MyAndromeda.Framework.Contexts;
using System.Collections.Generic;

namespace MyAndromeda.Web.Controllers.Api.Ibs
{
    public class IbsController : ApiController
    {
        private readonly IIbsService ibsService;
        private readonly IOrderHeaderDataService orderHeaderDataService;
        private readonly ICurrentSite currentSite;
        private readonly IAcsOrdersForIbsService acsOrdersForIbsService; 

        public IbsController(IIbsService ibsService, IOrderHeaderDataService orderHeaderDataService, ICurrentSite currentSite, IAcsOrdersForIbsService acsOrdersForIbsService) 
        {
            this.acsOrdersForIbsService = acsOrdersForIbsService;
            this.currentSite = currentSite;
            this.orderHeaderDataService = orderHeaderDataService;
            this.ibsService = ibsService;
        }

        private async Task<OrderHeader> GetOrderHeader(Guid orderId) 
        {
            var order = await
                orderHeaderDataService.OrderHeaders
                .AsNoTracking()
                .Include(e => e.Customer)
                .Include(e => e.Customer.Contacts)
                .Include(e => e.Customer.Address)
                .Include(e => e.CustomerAddress)
                .Include(e => e.OrderLines)
                .Where(e => e.ID == orderId)
                .SingleOrDefaultAsync();

            if (order.ExternalSiteID != this.currentSite.ExternalSiteId)
            {
                throw new Exception("No");
            }

            return order;
        }

        [Route("ibs/{andromedaSiteId}/getCustomer/{orderId}")]
        [HttpGet]
        public async Task<CustomerResultModel> GetCustomer(
            [FromUri]int andromedaSiteId, 
            [FromUri]Guid orderId) 
        {
            var order = await this.GetOrderHeader(orderId);

            var result = await ibsService.GetCustomerAsync(andromedaSiteId, order);

            return result;
        }

        [Route("ibs/{andromedaSiteId}/getMenu")]
        [HttpGet]
        public async Task<MenuResult> GetMenu(
            [FromUri]int andromedaSiteId) 
        {
            var result = await ibsService.GetMenuAsync(andromedaSiteId);

            return result;
        }

        [Route("ibs/{andromedaSiteId}/addCustomer/{orderId}")]
        [HttpGet]
        public async Task<CustomerResultModel> AddCustomer(
            [FromUri]int andromedaSiteId,
            [FromUri]Guid orderId)
        {
            var order = await this.GetOrderHeader(orderId);

            var result = await ibsService.AddCustomerAsync(andromedaSiteId, order);

            return result;
        }

        [Route("ibs/{andromedaSiteId}/previewOrder/{orderId}")]
        [HttpGet]
        public async Task<AddOrderRequest> ShowRequestAsync(
            [FromUri]int andromedaSiteId, 
            [FromUri]Guid orderId) 
        {
            var order = await this.GetOrderHeader(orderId);

            var customer = await ibsService.GetCustomerAsync(andromedaSiteId, order);
            if (customer == null) { customer = await ibsService.AddCustomerAsync(andromedaSiteId, order); }

            var result = await ibsService.CreateOrderData(andromedaSiteId, order, customer);

            return result;
        }

        [Route("ibs/{andromedaSiteId}/sendOrder/{orderId}")]
        [HttpGet]
        public async Task<AddOrderResult> SendRequestAsync(
            [FromUri]int andromedaSiteId,
            [FromUri]Guid orderId)
        {
            var order = await this.GetOrderHeader(orderId);
            var customer = await ibsService.GetCustomerAsync(andromedaSiteId, order);

            if (customer == null) { customer = await ibsService.AddCustomerAsync(andromedaSiteId, order); }

            var createOrderRequest = await ibsService.CreateOrderData(andromedaSiteId, order, customer);

            var result = await ibsService.AddOrderAsync(andromedaSiteId, order, customer, createOrderRequest);

            //done in addorderasync
            //await this.acsOrdersForIbsService.SaveSentOrderToIbsAsync(order, result);

            return result;
        }

        [Route("ibs/{andromedaSiteId}/getLocations")]
        [HttpGet]
        public async Task<Locations> GetLocations (
            [FromUri]int andromedaSiteId)
        {
            var result = await this.ibsService.GetLocations(andromedaSiteId);

            return result;
        }

        [Route("ibs/{andromedaSiteId}/getPaymentTypes")]
        [HttpGet]
        public async Task<IEnumerable<PaymentTypeModel>> GetPaymentTypes(
            [FromUri]int andromedaSiteId) 
        {
            var result = await this.ibsService.GetPaymentTypes(andromedaSiteId);

            return result;
        }


        //[Route("ibs/stores")]
        //[HttpGet]
        //public async Task<object> GetIbsStores()
        //{
        //    return null;
        //}

        //[Route("ibs/acs-orders")]
        //public async Task<object> AcsOrders() 
        //{
            
        //}
    }
}