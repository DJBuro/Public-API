using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Model;

namespace MyAndromeda.Data.DataAccess.DeliveryZone
{
    public class PostCodeSectorDataService : IPostCodeSectorDataService
    {
        private readonly AndroAdminDbContext dataContext;

        public PostCodeSectorDataService(AndroAdminDbContext dataContext)
        {
            this.dataContext = dataContext;
            this.Table = this.dataContext.Set<PostCodeSector>();
            this.TableQuery = this.Table
                                  .Include(e => e.DeliveryZoneName);
        }

        public DbSet<PostCodeSector> Table { get; private set; }

        public IQueryable<PostCodeSector> TableQuery { get; private set; }

        public IList<PostCodeSector> Get(int deliveryZoneNameId)
        {
            IList<PostCodeSector> deliveryAreaList = this.dataContext.PostCodeSectors.Where(e => e.DeliveryZoneId == deliveryZoneNameId).ToList();
            return deliveryAreaList;
        }

        public void Create(PostCodeSector postCodeSector)
        {
            if (postCodeSector != null)
            {
                PostCodeSector existingDeliveryArea = this.dataContext.PostCodeSectors.Where(e => e.DeliveryZoneId == postCodeSector.DeliveryZoneId && e.PostCodeSector1 == postCodeSector.PostCodeSector1).FirstOrDefault();
                if (existingDeliveryArea != null)
                {
                    this.Update(postCodeSector);
                }
                else
                {
                    this.dataContext.PostCodeSectors.Add(postCodeSector);
                    this.dataContext.SaveChanges();
                }
            }
        }

        public void Update(PostCodeSector postCodeSector)
        {
            PostCodeSector existing = this.Table.Find(postCodeSector.Id);
            ((IObjectContextAdapter)this.dataContext).ObjectContext.Detach(existing);
            this.dataContext.Entry(postCodeSector).State = EntityState.Modified;
            this.dataContext.SaveChanges();
        }

        public bool Delete(PostCodeSector postCodeSector)
        {
            var tableQuery = this.TableQuery.Where(e => e.Id == postCodeSector.Id);
            var result = tableQuery.SingleOrDefault();
            if (result != null)
            {
                PostCodeSector retObj = this.Table.Remove(postCodeSector);
                return this.dataContext.SaveChanges() > 0;
            }
            return false;
        }

        public bool Delete(int deliveryZoneNameId)
        {
            bool isDeleted = true;
            List<PostCodeSector> existingDeliveryAreas = this.dataContext.PostCodeSectors.Where(e => e.DeliveryZoneId == deliveryZoneNameId).ToList();

            if (existingDeliveryAreas != null)
            {
                int dataVersion = DataVersionHelper.GetNextDataVersion(this.dataContext);
                foreach (PostCodeSector psector in existingDeliveryAreas)
                {
                    isDeleted = (isDeleted && Delete(psector));
                }
            }
            return isDeleted;
        }

        public void ChangeIncludeScope<TPropertyModel>(System.Linq.Expressions.Expression<Func<PostCodeSector, TPropertyModel>> predicate)
        {
            throw new NotImplementedException();
        }

        public PostCodeSector New()
        {
            return new PostCodeSector() { };
        }

        public PostCodeSector Get(System.Linq.Expressions.Expression<Func<PostCodeSector, bool>> predicate)
        {
            IQueryable<PostCodeSector> tableQuery = this.TableQuery.Where(predicate);
            return tableQuery.SingleOrDefault();
        }

        public IQueryable<PostCodeSector> List()
        {
            return this.TableQuery;
        }

        public IQueryable<PostCodeSector> List(System.Linq.Expressions.Expression<Func<PostCodeSector, bool>> predicate)
        {
            return this.TableQuery.Where(predicate);
        }
    }
}