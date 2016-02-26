using MyAndromeda.Data.Model.AndroAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Controllers.Api.Store.Models
{
    public class StoreOccasionTimeModel
    {
        public string Description { get; set; }

        public DateTime EndUtc { get; set; }

        public Guid? Id { get; set; }

        public bool IsAllDay { get; set; }

        public string Occasions { get; set; }

        public string RecurrenceException { get; set; }

        public string RecurrenceRule { get; set; }

        public DateTime StartUtc { get; set; }

        public string Title { get; set; }

        public int StoreId { get; set; }
    }

    public static class StoreOccasionTimeModelExtensions 
    {
        public static StoreOccasionTimeModel ToViewModel(this StoreOccasionTime entity) 
        {
            var vm = new StoreOccasionTimeModel()
            {
                Description = entity.Description,
                EndUtc = entity.EndUtc,
                Id = entity.Id,
                IsAllDay = entity.IsAllDay,
                Occasions = entity.Occasions,
                RecurrenceException = entity.RecurrenceException,
                RecurrenceRule = entity.RecurrenceRule,
                StartUtc = entity.StartUtc,
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

            return entity;
        }

        public static void Update(this StoreOccasionTime entity, StoreOccasionTimeModel model) 
        {
            entity.Description = model.Description;
            entity.EndUtc = model.EndUtc;
            entity.Id = model.Id.GetValueOrDefault();
            entity.IsAllDay = model.IsAllDay;
            entity.Occasions = model.Occasions;
            entity.RecurrenceException = model.RecurrenceException;
            entity.RecurrenceRule = model.RecurrenceRule;
            entity.StartUtc = model.StartUtc;
            entity.StoreId = model.StoreId;
            entity.Title = model.Title;
        }
    }
}