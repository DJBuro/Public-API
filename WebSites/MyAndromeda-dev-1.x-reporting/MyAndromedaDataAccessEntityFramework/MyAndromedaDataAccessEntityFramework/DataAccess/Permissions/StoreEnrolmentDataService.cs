using System.Linq;
using MyAndromeda.Core.Site;
using MyAndromeda.Data.Model.MyAndromeda;
using System.Data.Entity;

namespace MyAndromeda.Data.DataAccess.Permissions
{
    public class StoreEnrolmentDataService : IStoreEnrolmentDataService
    {
        private readonly MyAndromedaDbContext dbContext;

        public StoreEnrolmentDataService(MyAndromedaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateStoreEnrolment(ISite site, IEnrolmentLevel enrolment)
        {
           
            DbSet<StoreEnrolment> table = this.dbContext.StoreEnrolments;
            StoreEnrolment enrolmentModel = table.Create();

            enrolmentModel.Active = true;
            enrolmentModel.EnrolmentLevelId = enrolment.Id;
            enrolmentModel.StoreId = site.Id;

            table.Add(enrolmentModel);

            this.dbContext.SaveChanges();
        }

        public void ClearEnrolements(ISite site)
        {
            DbSet<StoreEnrolment> table = this.dbContext.StoreEnrolments;
            IQueryable<StoreEnrolment> query = table.Where(e => e.StoreId == site.Id);
            StoreEnrolment[] results = query.ToArray();

            foreach (var result in results)
            {
                table.Remove(result);
            }

            this.dbContext.SaveChanges();
        }
    }
}