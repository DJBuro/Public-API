using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core.Services;
using MyAndromeda.Core.Site;
using MyAndromedaDataAccessEntityFramework.DataAccess.Permissions;

namespace MyAndromeda.Authorization.Services
{
    public class EnrolmentService : IEnrolmentService
    {
        private readonly IEnrolmentDataService enrolementDataService;
        private readonly IStoreEnrolmentDataService storeEnrolmentDataSerice;

        public EnrolmentService(IEnrolmentDataService enrolementDataService, IStoreEnrolmentDataService storeEnrolmentDataSerice)
        { 
            this.storeEnrolmentDataSerice = storeEnrolmentDataSerice;
            this.enrolementDataService = enrolementDataService;
        }

        public IEnumerable<IEnrolmentLevel> ListEnrolmentLevels()
        {
            return this.enrolementDataService.List().ToArray();
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
    }
}