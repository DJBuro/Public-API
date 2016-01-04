using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Customers;
using MyAndromeda.SendGridService.MarketingApi.Models.Recipients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using MyAndromeda.Services.Loyalty.Points;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using MyAndromeda.Framework.Helpers;

namespace MyAndromeda.Services.Marketing
{
    public class FindNewEventMarketingContacts : IRecipientListForEventMarketingService
    {
        private readonly ICustomerDataService customerDataService;
        private readonly IStoreDataService storeDataService;
        private readonly IAndromedaLoyaltyPointsHelper loyaltyPointsHelper;

        public FindNewEventMarketingContacts(ICustomerDataService customerDataService, IStoreDataService storeDataService, IAndromedaLoyaltyPointsHelper loyaltyPointsHelper)
        {
            this.loyaltyPointsHelper = loyaltyPointsHelper;
            this.storeDataService = storeDataService;
            this.customerDataService = customerDataService;
        }

        private IQueryable<Customer> GetMarketableEmailCustomers(int acsApplicationId, string marketingEventType)
        {
            var table = this.customerDataService.Customers;
            var query = table
                .Include(e=> e.CustomerLoyalties)
                .Where(e => e.ACSAplicationId == acsApplicationId)
                .Where(e => e.Contacts.Any(c => 
                    c.MarketingLevel.Name != "None" && c.ContactType.Name == "Email")
                )
                .Where(e => !e.Emails.Any(email => email.MarketingEventTypeName == marketingEventType));
                
            var now = DateTime.UtcNow;

            switch (marketingEventType) 
            {
                case MarketingEventTypes.NoOrders: {
                    query =
                        query
                            .Where(e=> e.OrderHeaders.Count == 0)
                            .Where(e => !e.Emails.Any(email => email.MarketingEventTypeName == marketingEventType));
                    break;
                }
                case MarketingEventTypes.OneWeekNoOrders: {

                    query = 
                        query
                            .Where(e => e.OrderHeaders.Count > 0)
                            .Where(e => e.OrderHeaders.All(order => DbFunctions.AddDays(order.TimeStamp, 7) < now))
                            .Where(e => !e.Emails.Any(email => email.MarketingEventTypeName == marketingEventType && DbFunctions.AddDays(email.TimeStampUtc, 7) < now));

                    break;
                }
                case MarketingEventTypes.OneMonthNoOrders: {

                    query = 
                        query
                            .Where(e => e.OrderHeaders.Count > 0)
                            .Where(e => e.OrderHeaders.All(order => DbFunctions.AddMonths(order.TimeStamp, 1) < now))
                            .Where(e => !e.OrderHeaders.All(order => DbFunctions.AddMonths(order.TimeStamp, 3) < now))
                            .Where(e => !e.Emails.Any(email => email.MarketingEventTypeName == marketingEventType && DbFunctions.AddMonths(email.TimeStampUtc, 1) < now));

                    break;
                }
                case MarketingEventTypes.ThreeMonthNoOrders: {

                    query = 
                        query
                            .Where(e => e.OrderHeaders.Count > 0)
                            .Where(e => e.OrderHeaders.All(order => DbFunctions.AddMonths(order.TimeStamp, 3) < now))
                            .Where(e => !e.Emails.Any(email => email.MarketingEventTypeName == marketingEventType && DbFunctions.AddMonths(email.TimeStampUtc, 3) < now));
                    
                    break;
                }
            }

            return query;
        }

        private async Task<List<Person>> ProcessQuery(IQueryable<Customer> query, int andromedaSiteId) 
        {
            var peopleQuery = query
                .Select(e => new
                {
                    CustomerId = e.ID,
                    Email = e.Contacts.FirstOrDefault(c => c.ContactType.Name == "Email").Value,
                    FirstName = e.FirstName,
                    CustomerLoyalty = e.CustomerLoyalties.FirstOrDefault(),//e.CustomerLoyalties.fir,
                    e.Title
                });

            var peopleQueryResult = await peopleQuery.ToArrayAsync();

            var loyaltySettings = await this.loyaltyPointsHelper.GetSettingsAsync(andromedaSiteId);
            var result = peopleQueryResult.Select(e => { 
                var person = new Person()
                {
                    CustomerId = e.CustomerId,
                    Email = e.Email,
                    Name = e.FirstName,
                    Title = e.Title
                };

                dynamic p = person;
                
                try
                {
                    //lower case to march the replacement field: [loyaltyPoints]
                    var loyaltyPoints = e.CustomerLoyalty != null
                        ? e.CustomerLoyalty.Points
                        : 0;

                    var loyaltySummary = loyaltyPointsHelper.GetPointsSummary(loyaltySettings, loyaltyPoints);

                    p.loyaltyPoints = loyaltySummary.Points;
                    p.loyaltyValue = loyaltySummary.Value;
                }
                catch (Exception)
                {
                    //dont care much if loyalty fails. 
                }


                return person;
            
            }).ToList();

            return result;
        }

        public async Task<List<Person>> GetPeopleForNoOrdersAsync(int acsApplicationId, int andromedaSiteId)
        {
            var query = this.GetMarketableEmailCustomers(acsApplicationId, MarketingEventTypes.NoOrders);

            var result = await this.ProcessQuery(query, andromedaSiteId);

            return result;
        }

        public async Task<List<Person>> GetCurrentCustomersAsync(int acsApplicationId, int andromedaSiteId)
        {
            var query = this.GetMarketableEmailCustomers(acsApplicationId, MarketingEventTypes.OneWeekNoOrders);

            var result = await this.ProcessQuery(query, andromedaSiteId);

            return result;
        }

        public async Task<List<Person>> GetLazyCustomersAsync(int acsApplicationId, int andromedaSiteId)
        {
            var query = this.GetMarketableEmailCustomers(acsApplicationId, MarketingEventTypes.OneMonthNoOrders);

            var result = await this.ProcessQuery(query, andromedaSiteId);

            return result;
        }

        public async Task<List<Person>> GetLapsedCustomersAsync(int acsApplicationId, int andromedaSiteId)
        {
            var query = this.GetMarketableEmailCustomers(acsApplicationId, MarketingEventTypes.ThreeMonthNoOrders);

            var result = await this.ProcessQuery(query, andromedaSiteId);

            return result;
        }

        public async Task<List<Person>> GetRecipients(MyAndromeda.Data.Model.MyAndromeda.MarketingEventCampaign model)
        {
            List<Person> people = new List<Person>();

            var store = await this.storeDataService.Table
                .Where(e => e.AndromedaSiteId == model.AndromedaSiteId).SingleOrDefaultAsync();

            var applicationIds = store.ACSApplicationSites.Select(e => e.ACSApplicationId).ToList();

            await applicationIds.ForEachAsync(async (e) =>
            {
                Task<List<MyAndromeda.SendGridService.MarketingApi.Models.Recipients.Person>> task = null;

                switch (model.TemplateName)
                {
                    case MarketingEventTypes.NoOrders:
                        {
                            task = this.GetPeopleForNoOrdersAsync(e, model.AndromedaSiteId);
                            break;
                        }
                    case MarketingEventTypes.OneWeekNoOrders:
                        {
                            task = this.GetCurrentCustomersAsync(e, model.AndromedaSiteId);
                            break;
                        }
                    case MarketingEventTypes.OneMonthNoOrders:
                        {
                            task = this.GetLazyCustomersAsync(e, model.AndromedaSiteId);
                            break;
                        }
                    case MarketingEventTypes.ThreeMonthNoOrders:
                        {
                            task = this.GetLapsedCustomersAsync(e, model.AndromedaSiteId);
                            break;
                        }
                }

                if (task == null)
                {
                    throw new Exception("No task exists for fetching recipients for: " + model.TemplateName);
                }

                if (task != null)
                {
                    var result = await task;
                    people.AddRange(result);
                }
            });

            return people;
        }
    }
}
