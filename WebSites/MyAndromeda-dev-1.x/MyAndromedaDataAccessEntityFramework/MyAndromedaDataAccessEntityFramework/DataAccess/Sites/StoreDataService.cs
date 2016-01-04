using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.Model;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

namespace MyAndromeda.Data.DataAccess.Sites
{
    public class StoreDataService : IStoreDataService 
    {
        private readonly AndroAdminDbContext androAdminDbContext;

        public StoreDataService(AndroAdminDbContext androAdminDbContext) 
        {
            this.androAdminDbContext = androAdminDbContext;
            this.Table = this.androAdminDbContext.Set<Store>();
            this.TableQuery = this.Table
                                  .Include(e => e.Address)
                                  .Include(e => e.Address.Country)
                                  .Include(e => e.Chain)
                                  .Include(e => e.StoreStatu);
        }

        public IDbSet<Store> Table { get; private set; }

        public IQueryable<Store> TableQuery { get; private set; }

        public void ChangeIncludeScope<TPropertyModel>(Expression<Func<Store, TPropertyModel>> predicate)
        {
            this.TableQuery = this.TableQuery.Include(predicate);
        }

        public Store New()
        {
            return new Store();
        }

        public Store Get(Expression<Func<Store, bool>> predicate)
        {
            return this.TableQuery.SingleOrDefault(predicate);
        }

        public IQueryable<Store> List()
        {
            return this.TableQuery;
        }

        public void Update(Store store) 
        {
            store.DataVersion = this.androAdminDbContext.GetNextDataVersionForEntity();

            this.androAdminDbContext.SaveChanges();
        }

        public IQueryable<Store> List(Expression<Func<Store, bool>> predicate)
        {
            return this.TableQuery.Where(predicate);
        }
    }
}