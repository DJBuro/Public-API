using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Framework.Dates;
using MyAndromeda.Framework.Helpers;
using MyAndromeda.Services.Ibs.Events;
using MyAndromeda.Services.Ibs.Models;
using MyAndromeda.Framework.Contexts;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

namespace MyAndromeda.Services.Ibs.Implementation
{
    public class CreateOrderService : ICreateOrderService
    {
        //notifiers / logging events mostyl.
        private readonly IIbsOrderEvents[] events;
        private readonly IIbsStoreDevice ibsStoreDevice;
        private readonly IStoreDataService siteServices;

        public CreateOrderService(IIbsOrderEvents[] events, DataWarehouseDbContext dataWarehouseDbContext,
            IStoreDataService siteServices,
            IIbsStoreDevice ibsStoreDevice)
        {
            this.TranslationTable = dataWarehouseDbContext.IbsRamesesTranslations;

            this.siteServices = siteServices;
            this.ibsStoreDevice = ibsStoreDevice;
            this.events = events;

            this.PaymentTypeTranslationTable = dataWarehouseDbContext.IbsPaymentTypeTranslations;
        }

        public IDbSet<IbsRamesesTranslation> TranslationTable { get; private set; }
        public IDbSet<IbsPaymentTypeTranslation> PaymentTypeTranslationTable { get; private set; }

        public async Task<AddOrderRequest> CreateOrderRequestModelAsync(int andromedaSiteId, OrderHeader orderHeader, CustomerResultModel customer)
        {
            await this.events.ForEachAsync(async (ev) =>
            {
                await ev.OrderCreatingAsync(orderHeader, customer);
            });

            //ibs device settings. 
            IbsStoreSettings device = await ibsStoreDevice.GetIbsStoreDeviceAsync(andromedaSiteId);

            //ibs <-> rameses menu ids
            IbsRamesesTranslation[] translationItems = await this.TranslationTable
                                             .Where(e => e.IbsCompanyId == device.CompanyCode)
                                             //.Select(e => new { e.PluNumber, e.RamesesMenuItemId })
                                             .ToArrayAsync();

            this.ValidateItemsAreAllThere(orderHeader, translationItems, customer);

            var store = await this.siteServices.Table
                .Where(e => e.AndromedaSiteId == andromedaSiteId)
                .Select(e=> new {
                    e.TimeZoneInfoId,
                    e.UiCulture
                })
                .FirstOrDefaultAsync();


            LocalizationContext localizationContext = LocalizationContext.Create(store.UiCulture, store.TimeZoneInfoId);
            IDateServices dateServices = DateServicesFactory.CreateInstance(localizationContext);
            
            //create a semi complete model
            AddOrderRequest model = orderHeader.TransformDraft(dateServices);

            //customer number returned from create / get customer
            model.CustomerNo = customer.CustomerNumber;

            //add food + match ibs to rameses
            model.AddFoodItems(orderHeader, translationItems);

            //add tip 
            model.AddOrGuessTip(orderHeader);

            //add discounts
            model.AddDiscounts(orderHeader);

            //add payment lines + match them to payment table. 
            var paymentTypes = await this.PaymentTypeTranslationTable
                .Where(e => e.IbsCompanyId == device.CompanyCode)
                .ToListAsync();

            model.AddPaymentLines(orderHeader, paymentTypes);

            await this.events.ForEachAsync(async (ev) =>
            {
                await ev.OrderRequestCreatedAsync(orderHeader, customer, model);
            });

            return model;
        }


        private async void ValidateItemsAreAllThere(OrderHeader orderHeader, IbsRamesesTranslation[] translationItems, CustomerResultModel customer) 
        {
            var someFoodItemsDontMatch = orderHeader.OrderLines.CheckForMatchingFoodIds(translationItems);

            if (someFoodItemsDontMatch.Any())
            {
                string ids = string.Join(",", someFoodItemsDontMatch);
                string message = string.Format("{0} - These ids cant be found in RamesesIbsTranslation: {1}", orderHeader.ExternalOrderRef, ids);

                var ex = new Exception(message);

                await this.events.ForEachAsync(async (ev) =>
                {
                    await ev.OrderCreatingFailedAsync(orderHeader, customer, ex);
                });

                throw ex;
            }
        }
    }
}