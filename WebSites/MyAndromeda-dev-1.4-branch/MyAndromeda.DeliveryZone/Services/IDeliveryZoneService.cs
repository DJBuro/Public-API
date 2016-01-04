using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAndromeda.Framework;
using MyAndromeda.Core;
using MyAndromedaDataAccessEntityFramework;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;

namespace MyAndromeda.Services.DeliveryZone.Services
{
    public interface IDeliveryZoneService  : IDependency
    {        
        IList<DeliveryArea> Get(int storeId);
        IList<DeliveryArea> GetListByExpression(int storeId, System.Linq.Expressions.Expression<Func<DeliveryArea, bool>> query);
        IQueryable<DeliveryArea> List();
        void Create(DeliveryArea deliveryArea);
        void Update(DeliveryArea deliveryArea);
        bool Delete(DeliveryArea deliveryArea);
        bool Delete(int storeId);
        DeliveryZoneName GetDeliveryZonesByRadius(int storeId);
        bool SaveDeliveryZones(DeliveryZoneName deliveryZone);
    }
}
