using MyAndromeda.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Ibs.Checks
{
    public class CheckOrderForIbsService : ICheckOrderIsActiveForIbsService
    {
        public CheckOrderForIbsService()
        {
        }

        /// <summary>
        /// Guid - order id; 
        /// int - Andromeda store id
        /// </summary>
        private readonly ConcurrentDictionary<Guid, int> orderProcessing = new ConcurrentDictionary<Guid, int>();

        public bool IsOrderProcessing(Guid orderHeaderId)
        {
            int andromeadSiteId;

            bool isProcessing = this.orderProcessing.TryGetValue(orderHeaderId, out andromeadSiteId);

            return isProcessing;
        }

        public void AddOrderToBeProcessed(Guid orderHeaderId, int andromedaStoreId)
        {
            this.orderProcessing.TryAdd(orderHeaderId, andromedaStoreId);
        }

        public void RemoveOrderFromProcessing(Guid orderHeaderId)
        {
            int andromeadSiteId;
            this.orderProcessing.TryRemove(orderHeaderId, out andromeadSiteId);

        }

    }
}
