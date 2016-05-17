using System;
using System.Linq;
using MyAndromeda.Data.DataWarehouse.Services;
using MyAndromeda.Data.DataWarehouse.Services.Customers;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.WebApiServices.Services
{
    public class OrderService : IOrderService 
    {
        private readonly ICurrentSite currentSite;
        private readonly ICustomerOrdersDataService customerOrdersDataService;
        private readonly ICustomerDataService customerDataService;
        private readonly IOrderStatusDataService orderStatusDataService;

        public OrderService(ICurrentSite currentSite, ICustomerOrdersDataService customerOrdersDataService, ICustomerDataService customerDataService, IOrderStatusDataService orderStatusDataService) 
        {
            this.orderStatusDataService = orderStatusDataService;
            this.customerDataService = customerDataService;
            this.customerOrdersDataService = customerOrdersDataService;
            this.currentSite = currentSite;
        }

        private OrderHeader GetOrder(int orderId) 
        {
            if (!currentSite.Available) { throw new NullReferenceException("CurrentSite in null"); }

            var query = this.customerOrdersDataService.List()
                            .Where(e => e.RamesesOrderNum == orderId)
                            .Where(e => e.ExternalSiteID == currentSite.ExternalSiteId);
            var result = query.SingleOrDefault();

            return result;
        }

        public Postal.Email CreateEmail(int orderId, Models.GprsPrinterIbacs model)
        {
            var site = this.currentSite.Site;

            var order = this.GetOrder(orderId);

            var customer = this.customerDataService
                            .List()
                            .Where(e => e.OrderHeaders.Any(header => header.ID == order.ID))
                            .FirstOrDefault();

            var contact = customer.Contacts.FirstOrDefault(e => e.ContactType.Name == "Email");

            Postal.Email email = null;

            email = model.Status == "1" ? 
                CreateSuccessMessage(contact, model, order) : 
                CreateFailedMessage(contact, model, order);

            return email;
        }

        public void UpodateOrderStatusHistory(OrderHeader order, OrderStatu status) 
        {
            this.orderStatusDataService.AddHistory(order, status);
        }

        public void UpdateOrderStatus(int orderId, bool success, string msg)
        {
            var allStatus = this.orderStatusDataService.List();
            var order = this.GetOrder(orderId);

            if (order == null) 
            {
                throw new NullReferenceException("Order is null"); 
            }
            
            var specificStatus = allStatus.Where(e=> e.Description == msg).FirstOrDefault();

            var oldStatus = order.OrderStatu;

            order.OrderStatu = specificStatus ?? 
                (success ? 
                //default for success
                allStatus.Single(e=> e.Description == DataWarehouseDefinitions.OrderStatus.OrderHasBeenCompleted) : 
                //default for failure
                allStatus.Single(e=> e.Description == DataWarehouseDefinitions.OrderStatus.OrderHasBeenCancelled));

            if (order.OrderStatu.Id != oldStatus.Id) 
            {
                this.orderStatusDataService.AddHistory(order, oldStatus);
            }

            this.customerOrdersDataService.Update(order); 
        }

        private Postal.Email CreateSuccessMessage(Contact contact, Models.GprsPrinterIbacs model, OrderHeader order) 
        {
            var successEmail = new Models.Email.SuccessMessage()
            {
                Site = this.currentSite.Site,
                Contact = contact,
                Customer = contact.Customer,
                Message = model.Msg,
                DeliveryTime = model.Delivery_time,
                Order = order
            };

            return successEmail;
        }

        private Postal.Email CreateFailedMessage(Contact contact, Models.GprsPrinterIbacs model, OrderHeader order) 
        {
            var failureEmail = new Models.Email.FailedMessage()
            {
                Order = order,
                Site = this.currentSite.Site,
                Contact = contact,
                Customer = contact.Customer,
                Message = model.Msg,
            };

            return failureEmail;
        }

        //public void UpdateOrderStatus(int orderId, bool success)
        //{
        //    var order = this.GetOrder(orderId);

        //    order.OrderStatu = 
        //}
    }
}