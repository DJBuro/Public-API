using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Core.Site;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Permissions
{
    public interface IStoreEnrolmentDataService : IDependency
    {
        void UpdateStoreEnrolment(ISite site, IEnrolmentLevel enrolment);
    }

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

            var table = dbContext.StoreEnrolments;
            var enrolmentModel = table.Create();

            enrolmentModel.Active = true;
            enrolmentModel.EnrolmentLevelId = enrolment.Id;
            enrolmentModel.StoreId = site.Id;

            table.Add(enrolmentModel);

            dbContext.SaveChanges();
        }

        public void ClearEnrolements(ISite site)
        {
            var table = dbContext.StoreEnrolments;
            var query = table.Where(e => e.StoreId == site.Id);
            var results = query.ToArray();

            foreach (var result in results)
            {
                table.Remove(result);
            }

            dbContext.SaveChanges();
        }
    }
}