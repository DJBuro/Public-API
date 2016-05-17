using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using AndroCloudDataAccessEntityFramework;
using AndroCloudDataAccessEntityFramework.Model;
using MyAndromeda.Framework.Contexts;


namespace MyAndromeda.Services.DeliveryZone.Services
{
    public class DeliveryZoneService : IDeliveryZoneService
    {
        private readonly ACSEntities datacontext;

        private readonly IDeliveryZoneService deliveryZoneDataService;
        private readonly IDeliveryZoneService deliveryDataService;
        private readonly ICurrentSite currentSite;

        public DeliveryZoneService(ACSEntities datacontext)
        {
            this.datacontext = datacontext;
            this.Table = this.datacontext.Set<DeliveryArea>();
        }
        
        public DbSet<DeliveryArea> Table { get; private set; }

        public DeliveryArea New()
        {
            throw new NotImplementedException();
        }

        public DeliveryArea Get(Guid Id)
        {
            throw new NotImplementedException();
        }

        public DeliveryArea GetByExpression(Expression<Func<DeliveryArea, bool>> query)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DeliveryArea> List()
        {
            throw new NotImplementedException();
        }

        public void Create(DeliveryArea voucher)
        {
            throw new NotImplementedException();
        }

        public void Update(DeliveryArea voucher)
        {
            throw new NotImplementedException();
        }

        public bool Delete(DeliveryArea voucher)
        {
            throw new NotImplementedException();
        }
    }
}
