using System.Linq;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.DataAccess;

namespace MyAndromeda.Data.DataAccess
{
    public class AcsApplicationDataService : IAcsApplicationDataService
    {
        private readonly AndroAdminDbContext dbContext;

        public AcsApplicationDataService(AndroAdminDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IQueryable<ACSApplication> Query()
        {
            return this.dbContext.ACSApplications;
        }

        public ACSApplication Get(int id)
        {
            var model = this.dbContext.ACSApplications.Find(id);

            return model;
        }

        public void Update(ACSApplication acsApplication)
        {
            this.dbContext.SaveChanges();
        }
    }
}