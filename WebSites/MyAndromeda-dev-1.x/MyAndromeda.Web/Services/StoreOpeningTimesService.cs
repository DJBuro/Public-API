using System.Collections.Generic;
using System.Web.Mvc;
using MyAndromeda.Core;
using System;
using System.Linq;
using MyAndromeda.Data.Domain;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Framework.Contexts;
using MyAndromedaDataAccess.DataAccess;
using MyAndromeda.Web.Areas.Store.Models;
using MyAndromeda.Web.Helper;
using MyAndromeda.CloudSynchronization.Services;

namespace MyAndromeda.Web.Services
{
    public interface IStoreOpeningTimesService : IDependency
    {
        void AddOpeningTime(string day, TimeSpan startTimeSpan, TimeSpan endTimeSpan, ModelStateDictionary modelState);

        OpeningTimesForTheWeek GetOpenTimes();

        void DeleteOpeningTime(int id);

        void DeleteAllTimesForDay(string day);

        void AddOpeningTime(TimeSpanBlock timeSpanBlock);

    }

    public class StoreOpeningTimesService : IStoreOpeningTimesService
    {
        private readonly ICurrentSite currentSite;
        private readonly ICurrentUser currentUser;
        private readonly IOpeningHoursDataAccess openingHoursDataAccess;
        private readonly ISynchronizationTaskService acsSynchronizationTaskService;
        
        public StoreOpeningTimesService(ICurrentSite currentSite, ICurrentUser user, IOpeningHoursDataAccess openingHoursDataAccess, ISynchronizationTaskService acsSynchronizationTaskService)
        {
            this.acsSynchronizationTaskService = acsSynchronizationTaskService;
            this.openingHoursDataAccess = openingHoursDataAccess;
            this.currentUser = user;
            this.currentSite = currentSite;
        }
        
        public void AddOpeningTime(string day, TimeSpan startTimeSpan, TimeSpan endTimeSpan, ModelStateDictionary modelState)
        {
            var allOpenTimes = this.GetOpenTimes();

            var model = new TimeSpanBlock()
            {
                Day = day,
                OpenAllDay = false,
                StartTime = startTimeSpan.Hours.ToString("00") + ":" + startTimeSpan.Minutes.ToString("00"),
                EndTime = endTimeSpan.Hours.ToString("00") + ":" + endTimeSpan.Minutes.ToString("00")
            };

            var validModel = OpeningTimesHelper.CheckNewOpeningTime(allOpenTimes, day, startTimeSpan, endTimeSpan, (failure) => {
                modelState.AddModelError("StartTime", failure);
            });

            if (validModel)
            {
                //remove the open all day ... adding specific time slots
                if (allOpenTimes.ContainsKey(day)) {
                    var remove = this.openTimes[day].Where(e => e.OpenAllDay);
                    foreach (var item in remove) { this.DeleteOpeningTime(item.Id); }
                }

                this.AddOpeningTime(model);
            }
        }

        private OpeningTimesForTheWeek openTimes; 
        public OpeningTimesForTheWeek GetOpenTimes() 
        {
            if (openTimes != null) { return openTimes; }

            this.openTimes = new OpeningTimesForTheWeek();

            var models = this.openingHoursDataAccess.ListBySiteId(this.currentSite.SiteId);

            foreach (var model in models) 
            {
                var key = model.Day;

                if(!this.openTimes.ContainsKey(key))
                {
                    this.openTimes.Add(key, new List<TimeSpanBlock>());
                }

                this.openTimes[key].Add(model);
            }

            foreach (List<TimeSpanBlock> blocks in openTimes.Values)
            {
                blocks.Sort(new TimeSpanBlockComparer());
            }

            return this.openTimes;
        }

        public void DeleteOpeningTime(int id)
        {
            this.openingHoursDataAccess.DeleteById(this.currentSite.SiteId, id);
            this.Sync("Deleted a opening time.");
        }

        public void DeleteAllTimesForDay(string day)
        {
            this.openingHoursDataAccess.DeleteBySiteIdDay(this.currentSite.SiteId, day);
            this.Sync("Deleted all opening times.");
        }

        public void AddOpeningTime(TimeSpanBlock timeSpanBlock)
        {
            this.openingHoursDataAccess.Add(this.currentSite.SiteId, timeSpanBlock);
            this.Sync("Added a opening time.");
        }

        private void Sync(string action) 
        {
            this.acsSynchronizationTaskService.CreateTask(new CloudSynchronizationTask()
            {
                Completed = false,
                Description = action,
                Name = "Site Update : Store opening times",
                Timestamp = DateTime.UtcNow,
                StoreId = this.currentSite.SiteId,
                InvokedByUserId = this.currentUser.User.Id,
                InvokedByUserName = this.currentUser.User.Username
            });
        }
    }
}