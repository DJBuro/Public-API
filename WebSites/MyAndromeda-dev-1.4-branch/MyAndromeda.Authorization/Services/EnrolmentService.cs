using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core.Services;
using MyAndromeda.Core.Site;
using MyAndromedaDataAccessEntityFramework.DataAccess.Permissions;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Authorization.Services
{
    public class EnrolmentService : IEnrolmentService
    {
        private readonly IEnrolmentDataService enrolementDataService;
        private readonly IStoreEnrolmentDataService storeEnrolmentDataSerice;
        private readonly ICoreStoreEnrollments coreEnrollments;

        public EnrolmentService(IEnrolmentDataService enrolementDataService, IStoreEnrolmentDataService storeEnrolmentDataSerice, ICoreStoreEnrollments coreEnrollments)
        { 
            this.coreEnrollments = coreEnrollments;
            this.storeEnrolmentDataSerice = storeEnrolmentDataSerice;
            this.enrolementDataService = enrolementDataService;
        }

        

        public IEnumerable<IEnrolmentLevel> ListEnrolmentLevels()
        {
            var localList = this.coreEnrollments.GetEnrollments();

            var dbList = this.enrolementDataService
                .List()
                .ToList();

            foreach (var item in localList) 
            {
                if (dbList.Any(e => e.Name.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase))) { continue; }
                var newIem = this.enrolementDataService.Create(item.Name, string.Empty);

                dbList.Add(newIem);
            }

            return dbList;
        }

        public IEnrolmentLevel GetEnrolmentLevel(string name) 
        {
            var enrolment = this.enrolementDataService.Query(e => e.Name == name).SingleOrDefault();

            if (enrolment == null)
            {
                var level = this.enrolementDataService.Create(name, name);

                return level;
            }

            return enrolment;
        }

        public IEnumerable<IEnrolmentLevel> GetEnrolmentLevels(ISite site)
        {
            var dbEnrolments = this.enrolementDataService
                                   .Query(e => e.StoreEnrolments.Any(enrolment => enrolment.StoreId == site.Id))
                                   .ToArray();

            var defaultStoreEnrolment = new[] { this.GetEnrolmentLevel("Default Store") };

            return dbEnrolments.Union(defaultStoreEnrolment);
        }

        //public IEnumerable<IEnrolmentLevel> GetEnrolmentLevels(int storeId)
        //{
        //    //return this.enrolementDataService.Query(e => e.StoreEnrolments.Any(storeEnrolement => storeEnrolement.Active && storeEnrolement.StoreId == storeId)).ToArray();
        //}
        public void UpdateSitesEnrolment(ISite site, IEnrolmentLevel enrolmentLevel)
        {
            this.storeEnrolmentDataSerice.UpdateStoreEnrolment(site, enrolmentLevel);
        }

        public void CreateORUpdate(IEnrolmentLevel enrolmenLevel)
        {
            if (enrolmenLevel.Id > 0) 
            {
                this.enrolementDataService.Update(enrolmenLevel);
                return;
            }

            var model = this.enrolementDataService.Create(enrolmenLevel.Name, enrolmenLevel.Description);
            enrolmenLevel.Id = model.Id;
        }

        public void Delete(IEnrolmentLevel enrollment)
        {
            this.enrolementDataService.Delete(enrollment);
        }
    }
}