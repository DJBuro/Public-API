using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Data.DataAccess.DeliveryZone
{
    public interface IDeliveryZoneNameDataService : IDataProvider<DeliveryZoneName>, IDependency
    {
        IList<DeliveryZoneName> Get(int storeId);

        void Create(DeliveryZoneName deliveryZoneName);

        void Update(DeliveryZoneName deliveryZoneName);

        bool Delete(DeliveryZoneName deliveryZoneName);

        bool Delete(int storeId);
    }
}
