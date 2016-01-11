using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Framework.Dates;
using MyAndromeda.Framework.Helpers;
using MyAndromeda.Services.Ibs.Events;
using MyAndromeda.Services.Ibs.Models;

namespace MyAndromeda.Services.Ibs.Implementation
{
    public class CreateOrderService : ICreateOrderService
    {
        private readonly IIbsOrderEvents[] events;
        private readonly IDateServices dateServices;
        private readonly IIbsStoreDevice ibsStoreDevice;

        public CreateOrderService(IIbsOrderEvents[] events, IDateServices dateServices, DataWarehouseDbContext dataWarehouseDbContext, IIbsStoreDevice ibsStoreDevice)
        {
            this.ibsStoreDevice = ibsStoreDevice;
            this.dateServices = dateServices;
            this.events = events;

            this.TranslationTable = dataWarehouseDbContext.IbsRamesesTranslations;
        }

        public IDbSet<IbsRamesesTranslation> TranslationTable { get; private set; }

        public async Task<AddOrderRequest> CreateOrderRequestModelAsync(int andromedaSiteId, OrderHeader orderHeader, CustomerResultModel customer)
        {
            await this.events.ForEachAsync(async (ev) =>
            {
                await ev.OrderCreatingAsync(orderHeader, customer);
            });

            var device = await ibsStoreDevice.GetIbsStoreDeviceAsync(andromedaSiteId);
            var translationItems = await this.TranslationTable
                                             .Where(e => e.IbsCompanyId == device.CompanyCode)
                                             .Select(e => new { e.PluNumber, e.RamesesMenuItemId })
                                             .ToArrayAsync();

            var orderItems = orderHeader.OrderLines.ToArray();

            var someDontMatch = orderItems
                                          .Where(e => !translationItems.Any(k => k.RamesesMenuItemId == e.ProductID))
                                          .Select(e => e.ProductID)
                                          .ToArray();

            if (someDontMatch.Length > 0)
            {
                string ids = string.Join(",", someDontMatch);

                string message = string.Format("{0}- These ids cant be found in RamesesIbsTranslation: {1}", orderHeader.ExternalOrderRef, ids);

                var ex = new Exception(message);

                await this.events.ForEachAsync(async (ev) =>
                {
                    await ev.OrderCreatingFailedAsync(orderHeader, customer, ex);
                });

                throw ex;
            }

            var model = orderHeader.Transform(dateServices);

            //convert id to offset.
            foreach (var item in model.Items.Where(e => e.m_eLineType == IbsWebOrderApi.eWebOrderLineType.ePLU))
            {
                //m_lOffset is currently the product id
                var pluNumber = translationItems
                                                .Where(e => e.RamesesMenuItemId == item.m_lOffset)
                                                .Select(e => e.PluNumber)
                                                .First();

                item.m_lOffset = pluNumber;
            }

            model.CustomerNo = customer.CustomerNumber;

            await this.events.ForEachAsync(async (ev) =>
            {
                await ev.OrderRequestCreatedAsync(orderHeader, customer, model);
            });

            return model;
        }
    }
}