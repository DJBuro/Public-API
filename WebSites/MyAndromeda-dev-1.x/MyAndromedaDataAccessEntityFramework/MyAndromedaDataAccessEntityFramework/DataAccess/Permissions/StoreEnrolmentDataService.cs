using System.Linq;
using MyAndromeda.Core.Site;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Permissions
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
            this.ClearEnrolements(site);

            var table = this.dbContext.StoreEnrolments;
            var enrolmentModel = table.Create();

            enrolmentModel.Active = true;
            enrolmentModel.EnrolmentLevelId = enrolment.Id;
            enrolmentModel.StoreId = site.Id;

            table.Add(enrolmentModel);

            this.dbContext.SaveChanges();
        }

        public void ClearEnrolements(ISite site)
        {
            var table = this.dbContext.StoreEnrolments;
            var query = table.Where(e => e.StoreId == site.Id);
            var results = query.ToArray();

            foreach (var result in results)
            {
                table.Remove(result);
            }

            this.dbContext.SaveChanges();
        }
    }
}