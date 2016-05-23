using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Data.DataAccess.DeliveryZone;

namespace MyAndromeda.DeliveryZone.Services
{
    public class DeliveryZoneService : IDeliveryZoneService
    {
        private readonly IDeliveryZoneDataService deliveryZoneDataService; // add the new dataservices here
        private readonly ICurrentSite currentSite;

        public DeliveryZoneService(IDeliveryZoneDataService deliveryZoneDataService,
            ICurrentSite currentSite)
        {
            this.deliveryZoneDataService = deliveryZoneDataService;
            this.currentSite = currentSite;
        }

        public IList<DeliveryArea> GetListByExpression(int storeId, Expression<Func<DeliveryArea, bool>> predicate)
        {
            var deliveryAreas = this.List().Where(predicate).ToList();

            return deliveryAreas;
        }

        public IList<DeliveryArea> Get(int storeId)
        {
            IList<DeliveryArea> deliveryAreas = this.deliveryZoneDataService.Get(storeId);

            return deliveryAreas;

        }

        public void Create(DeliveryArea deliveryArea)
        {
            this.deliveryZoneDataService.Create(deliveryArea);
        }
        public void Update(DeliveryArea deliveryArea)
        {
            this.deliveryZoneDataService.Update(deliveryArea);
        }
        public bool Delete(DeliveryArea deliveryArea)
        {
            return this.deliveryZoneDataService.Delete(deliveryArea);
        }


        public IQueryable<DeliveryArea> List()
        {
            if (!this.currentSite.Available)
            {
                return Enumerable.Empty<DeliveryArea>().AsQueryable();
            }

            return this.deliveryZoneDataService.List(e => e.StoreId == currentSite.SiteId);
        }

        public bool Delete(int storeId)
        {
            return this.deliveryZoneDataService.Delete(storeId);
        }

        public DeliveryZoneName GetDeliveryZonesByRadius(int storeId)
        {
            return this.deliveryZoneDataService.GetDeliveryZonesByRadius(storeId);
        }

        public bool SaveDeliveryZones(DeliveryZoneName deliveryZone)
        {
            return this.deliveryZoneDataService.SaveDeliveryZones(deliveryZone);
        }

    }
}
