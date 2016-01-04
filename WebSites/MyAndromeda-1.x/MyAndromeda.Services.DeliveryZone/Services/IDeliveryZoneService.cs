using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroCloudDataAccessEntityFramework.Model;

namespace MyAndromeda.Services.DeliveryZone.Services
{
   public interface IDeliveryZoneService : IDependency
    {
        DeliveryArea New();
        DeliveryArea Get(System.Guid Id);
        DeliveryArea GetByExpression(System.Linq.Expressions.Expression<Func<DeliveryArea, bool>> query);
        IQueryable<DeliveryArea> List();
        void Create(DeliveryArea voucher);
        void Update(DeliveryArea voucher);
        bool Delete(DeliveryArea voucher); 

    }
}
