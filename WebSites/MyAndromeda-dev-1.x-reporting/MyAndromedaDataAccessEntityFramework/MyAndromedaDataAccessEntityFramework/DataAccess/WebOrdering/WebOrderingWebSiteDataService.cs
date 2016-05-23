using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.DataAccess.WebOrdering;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Data.DataAccess.WebOrdering
{
    public class WebOrderingWebSiteDataService : IWebOrderingWebSiteDataService
    {
        private readonly AndroAdminDbContext dataContext;

        public WebOrderingWebSiteDataService(AndroAdminDbContext dataContext)
        {
            this.dataContext = dataContext;
            this.Table = this.dataContext.Set<AndroWebOrderingWebsite>();
            this.TableQuery = this.Table
                                  .Include(e => e.ACSApplication)
                                  .Include(e => e.Chain);
        }

        public DbSet<AndroWebOrderingWebsite> Table { get; private set; }

        public IQueryable<AndroWebOrderingWebsite> TableQuery { get; private set; }

        public void Update(AndroWebOrderingWebsite website)
        {
            AndroWebOrderingWebsite existing = this.Table.Find(website.Id);
            ((IObjectContextAdapter)this.dataContext).ObjectContext.Detach(existing);
            this.dataContext.Entry(website).State = EntityState.Modified;
            this.dataContext.SaveChanges();
        }

        public void ChangeIncludeScope<TPropertyModel>(Expression<Func<AndroWebOrderingWebsite, TPropertyModel>> predicate)
        {
            this.TableQuery =
                this.TableQuery.Include(predicate);
        }

        public AndroWebOrderingWebsite New()
        {
            throw new NotImplementedException();
        }

        public AndroWebOrderingWebsite Get(Expression<Func<AndroWebOrderingWebsite, bool>> predicate)
        {
            var table = this.TableQuery;
            var tableQuery = table.Where(predicate);

            return tableQuery.FirstOrDefault();
        }

        public IQueryable<AndroWebOrderingWebsite> List()
        {
            return this.TableQuery;
        }

        public IQueryable<AndroWebOrderingWebsite> List(Expression<Func<AndroWebOrderingWebsite, bool>> predicate)
        {
            return this.TableQuery.Where(predicate);
        }

        public AndroWebOrderingWebsite GetWebOrderingSiteForOrder(int applicationId, string externalSiteId)
        {
            var site = this.dataContext.Stores.Where(e => e.ExternalId.Equals(externalSiteId, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            AndroWebOrderingWebsite website = null;

            if (site != null)
            {
                var application = this.dataContext.ACSApplications.Include(e => e.ACSApplicationSites).Include(e => e.AndroWebOrderingWebsites).Where(e => e.Id == applicationId && e.ACSApplicationSites.Where(a => a.SiteId == site.Id).Count() > 0).FirstOrDefault();
                website = application != null ? application.AndroWebOrderingWebsites.FirstOrDefault() : website;
            }

            return website;
        }

        public void Delete(AndroWebOrderingWebsite website)
        {
            this.Table.Remove(website);
            this.dataContext.SaveChanges();
        }
    }
}