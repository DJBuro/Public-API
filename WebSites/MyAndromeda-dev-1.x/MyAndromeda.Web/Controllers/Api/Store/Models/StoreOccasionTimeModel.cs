﻿using MyAndromeda.Data.Model.AndroAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Controllers.Api.Store.Models
{
    public class StoreOccasionTimeModel
    {
        public DateTime End { get; set; }

        public Guid? Id { get; set; }

        public bool IsAllDay { get; set; }

        public string[] Occasions { get; set; }

        public string RecurrenceException { get; set; }

        public string RecurrenceRule { get; set; }

        public DateTime Start { get; set; }

        public string Title { get; set; }

        public int StoreId { get; set; }
    }

    public static class StoreOccasionTimeModelExtensions 
    {
        public static StoreOccasionTimeModel ToViewModel(this StoreOccasionTime entity) 
        {
            var vm = new StoreOccasionTimeModel()
            {
                End = entity.EndUtc,
                Id = entity.Id,
                IsAllDay = entity.IsAllDay,
                Occasions = entity.Occasions.Split(','),
                RecurrenceException = entity.RecurrenceException,
                RecurrenceRule = entity.RecurrenceRule,
                Start = entity.StartUtc,
                StoreId = entity.StoreId,
                Title = entity.Title,
            };

            return vm;
        }


        public static StoreOccasionTime CreateEntiy(this StoreOccasionTimeModel model, MyAndromeda.Data.Model.AndroAdmin.Store storeEntity) 
        {
            var entity = new StoreOccasionTime();
            entity.Id = Guid.NewGuid();
            entity.StoreId = storeEntity.Id;

            entity.Update(model);

            return entity;
        }

        public static void Update(this StoreOccasionTime entity, StoreOccasionTimeModel model) 
        {
            entity.EndUtc = model.End;
            //entity.Id = model.Id.GetValueOrDefault();
            entity.IsAllDay = model.IsAllDay;
            entity.Occasions = string.Join(",", model.Occasions);
            entity.RecurrenceException = model.RecurrenceException;
            entity.RecurrenceRule = model.RecurrenceRule;
            entity.StartUtc = model.Start;
            //entity.StoreId = model.StoreId;
            entity.Title = model.Title;

            entity.StartTimezone = string.Empty;
            entity.EndTimezone = string.Empty;
        }
    }
}