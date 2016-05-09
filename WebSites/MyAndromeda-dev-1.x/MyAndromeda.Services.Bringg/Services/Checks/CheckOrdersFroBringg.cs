using System;
using System.Linq;
using System.Collections.Generic;
using MyAndromeda.Core;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MyAndromeda.Services.Bringg.Services.Checks
{

    public abstract class CheckOrdersFroBringg
    {
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