using System;
using System.Linq;
using MyAndromeda.Data.DataAccess.WebOrdering;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Customers;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;
using MyAndromeda.Framework.Logging;

namespace MyAndromeda.Services.Orders
{
    public class OrderEmailingService : IOrderEmailingService
    {
        private readonly ICurrentSite currentSite;
        private readonly ICustomerOrdersDataService customerOrdersDataService;
        private readonly ICustomerDataService customerDataService;
        private readonly IWebOrderingWebSiteDataService WebOrderingService;
        private readonly IMyAndromedaLogger logger;

        public OrderEmailingService(ICurrentSite currentSite, ICustomerOrdersDataService customerOrdersDataService, ICustomerDataService customerDataService, IWebOrderingWebSiteDataService WebOrderingService, IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.customerDataService = customerDataService;
            this.customerOrdersDataService = customerOrdersDataService;
            this.currentSite = currentSite;
            this.WebOrderingService = WebOrderingService;            
        }

        private OrderHeader GetOrder(string externalSiteId, int ramesesOrderId)
        {
            if (!currentSite.Available)
            {
                throw new NullReferenceException("CurrentSite in null");
            }

            this.customerOrdersDataService.ChangeIncludeScope(e => e.OrderStatu);
            var query = this.customerOrdersDataService.List()
                            .OrderByDescending(e=> e.TimeStamp)
                            .Where(e => e.RamesesOrderNum == ramesesOrderId)
                            .Where(e => e.ExternalSiteID == externalSiteId);

            OrderHeader result = null;

            var results = query.ToArray();

            if (results.Length == 0) 
            {
                this.logger.Error("No order could be found externalSiteId: {0}; rameses order id: {1}", externalSiteId, ramesesOrderId);
                throw new Exception("No order could be found");
            }
            if (results.Length > 1) 
            {
                this.logger.Error("There are too many orders: {0}; rameses order id: {1}", externalSiteId, ramesesOrderId);
            }

            try
            {
                result = query.First();    
            }
            catch (Exception e)
            {
                var sql = query.ToTraceQuery();

                this.logger.Error("Cant find single order with rameses order id: " + ramesesOrderId);
                this.logger.Error(sql);

                throw e;
            }

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

        private Postal.Email CreateSuccessMessage(Contact contact, string message, string deliveryTime, OrderHeader order, 
            AndroWebOrderingWebsite websiteCofig)
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

        private Postal.Email CreateFailedMessage(Contact contact, string failureMessage, 
            OrderHeader order, 
            AndroWebOrderingWebsite websiteCofig)
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