using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using MyAndromeda.Core;
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

}