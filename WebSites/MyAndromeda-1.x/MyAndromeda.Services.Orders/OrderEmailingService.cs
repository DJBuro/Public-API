using System;
using System.Linq;
using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services;
using MyAndromeda.Data.DataWarehouse.Services.Customers;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Framework.Contexts;
using MyAndromedaDataAccessEntityFramework.DataAccess.WebOrdering;
using MyAndromeda.Services.Orders.Events;


namespace MyAndromeda.Services.Orders
{
    public class OrderEmailingService : IOrderEmailingService
    {
        private readonly ICurrentSite currentSite;
        private readonly ICustomerOrdersDataService customerOrdersDataService;
        private readonly ICustomerDataService customerDataService;
        private readonly IWebOrderingWebSiteDataService WebOrderingService;        

        public OrderEmailingService(ICurrentSite currentSite, ICustomerOrdersDataService customerOrdersDataService, ICustomerDataService customerDataService, IWebOrderingWebSiteDataService WebOrderingService)
        {
            this.customerDataService = customerDataService;
            this.customerOrdersDataService = customerOrdersDataService;
            this.currentSite = currentSite;
            this.WebOrderingService = WebOrderingService;            
        }

        private OrderHeader GetOrder(string externalSiteId, int orderId)
        {
            if (!currentSite.Available)
            {
                throw new NullReferenceException("CurrentSite in null");
            }

            this.customerOrdersDataService.ChangeIncludeScope(e => e.OrderStatu);
            var query = this.customerOrdersDataService.List()
                            .Where(e => e.RamesesOrderNum == orderId)
                            .Where(e => e.ExternalSiteID == externalSiteId);

            var result = query.SingleOrDefault();

            return result;
        }

        public Postal.Email CreateEmail(string externalSiteId, int ramesesOrderId, string message, string deliveryTime, bool success)
        {
            var order = this.GetOrder(externalSiteId, ramesesOrderId);

            var customer = this.customerDataService
                               .List()
                               .Where(e => e.OrderHeaders.Any(header => header.ID == order.ID))
                               .FirstOrDefault();

            var contact = customer.Contacts.FirstOrDefault(e => e.ContactType.Name == "Email");

            //var websiteConfig = WebOrderingService.GetWebOrderingSiteForOrder(order.ApplicationID, externalSiteId);

            var websiteConfig = currentSite.AndroWebOrderingSites.FirstOrDefault();

            Postal.Email email = null;

            email = success ?
                    CreateSuccessMessage(contact, message, deliveryTime, order, websiteConfig) :
                    CreateFailedMessage(contact, message, order, websiteConfig);

            return email;
        }

        private Postal.Email CreateSuccessMessage(Contact contact, string message, string deliveryTime, OrderHeader order, MyAndromedaDataAccessEntityFramework.Model.AndroAdmin.AndroWebOrderingWebsite websiteCofig)
        {
            var successEmail = new Emails.SuccessMessage()
            {
                Site = this.currentSite.Site,
                Store = this.currentSite.Store,
                Contact = contact,
                Customer = contact.Customer,
                Message = message,
                DeliveryTime = deliveryTime,
                Order = order,
                WebsiteStuff = websiteCofig
            };

            return successEmail;
        }

        private Postal.Email CreateFailedMessage(Contact contact, string failureMessage, OrderHeader order, MyAndromedaDataAccessEntityFramework.Model.AndroAdmin.AndroWebOrderingWebsite websiteCofig)
        {
            var failureEmail = new Emails.FailedMessage()
            {
                Order = order,
                Site = this.currentSite.Site,
                Store = this.currentSite.Store,
                Contact = contact,
                Customer = contact.Customer,
                Message = failureMessage,
                WebsiteStuff = websiteCofig
            };

            return failureEmail;
        }
    }
}