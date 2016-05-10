using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Data.DataAccess.DeliveryZone
{
    public interface IDeliveryZoneDataService : IDataProvider<DeliveryArea>, IDependency
    {
        IList<DeliveryArea> Get(int storeId);

        void Create(DeliveryArea deliveryArea);

        void Update(DeliveryArea deliveryArea);

        bool Delete(DeliveryArea deliveryArea);

        bool Delete(int storeId);

        DeliveryZoneName GetDeliveryZonesByRadius(int storeId);

        bool SaveDeliveryZones(DeliveryZoneName deliveryZone);
    }
}
