using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Data.DataAccess.WebOrdering
{
    public class WebOrderingThemeDataService : IWebOrderingThemeDataService
    {
        private readonly AndroAdminDbContext dataContext;

        public WebOrderingThemeDataService(AndroAdminDbContext dataContext)
        {
            this.dataContext = dataContext;
            this.Table = this.dataContext.Set<AndroWebOrderingTheme>();
            this.TableQuery = this.Table;
        }

        public DbSet<AndroWebOrderingTheme> Table { get; private set; }

        public IQueryable<AndroWebOrderingTheme> TableQuery { get; private set; }

        public void ChangeIncludeScope<TPropertyModel>(System.Linq.Expressions.Expression<Func<AndroWebOrderingTheme, TPropertyModel>> predicate)
        {
            throw new NotImplementedException();
        }

        public AndroWebOrderingTheme New()
        {
            return this.Table.Create();
        }

        public AndroWebOrderingTheme Get(System.Linq.Expressions.Expression<Func<AndroWebOrderingTheme, bool>> predicate)
        {
            var table = this.TableQuery;
            var tableQuery = table.Where(predicate);
            
            return tableQuery.SingleOrDefault();
        }

        public IQueryable<AndroWebOrderingTheme> List()
        {
            return this.TableQuery;
        }

        public IQueryable<AndroWebOrderingTheme> List(System.Linq.Expressions.Expression<Func<AndroWebOrderingTheme, bool>> predicate)
        {
            return this.TableQuery.Where(predicate);
        }
    }
}