﻿using Kendo.Mvc.UI;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Controllers.Api.Hr.Models
{
    public static class EmployeeScheduleModelExtensions
    {
        public static EmployeeScheduleModel ToModel(this EmployeeSchedule entity) 
        {

            return new EmployeeScheduleModel()
            {
                AndromedaSiteId = entity.AndromedaSiteId,
                Description = entity.Description,
                EmployeeId = entity.EmployeeRecordId,
                End = entity.EndUtc,
                EndTimezone = entity.EndTimezone,
                Id = entity.Id,
                IsAllDay = entity.IsAllDay,
                RecurrenceException = entity.RecurrenceException,
                RecurrenceRule = entity.RecurrenceRule,
                Start = entity.StartUtc,
                StartTimezone = entity.StartTimezone,
                TaskType = entity.TaskType,
                Title = entity.Title
            };

        }

        public static void UpdateFromModel(this EmployeeSchedule entity, EmployeeScheduleModel model)
        {
            entity.AndromedaSiteId = model.AndromedaSiteId;
            entity.Description = model.Description;
            entity.EmployeeRecordId = model.EmployeeId;
            entity.EndTimezone = model.EndTimezone;
            entity.EndUtc = model.End;
            entity.IsAllDay = model.IsAllDay;
            entity.RecurrenceException = model.RecurrenceException;
            entity.RecurrenceRule = model.RecurrenceRule;
            entity.StartTimezone = model.StartTimezone;
            entity.StartUtc = model.Start;
            entity.TaskType = model.TaskType;
            entity.Title = model.Title;
        }

        public static EmployeeSchedule CreateEntiyFromModel(this EmployeeScheduleModel model) 
        {
            var entity = new EmployeeSchedule();
            entity.UpdateFromModel(model);

            entity.Id = model.Id.GetValueOrDefault(Guid.NewGuid());

            return entity;
        }
    }

    public class EmployeeScheduleModel : ISchedulerEvent
    {
        public Guid? Id { get; set; }

        //[JsonProperty("title")]
        public string Title
        {
            get;
            set;
        }

        //[JsonProperty("endTimezone")]
        public string EndTimezone
        {
            get;
            set;
        }

        //[JsonProperty("description")]
        public string Description
        {
            get;
            set;
        }

        //[JsonProperty("isAllDay")]
        public bool IsAllDay
        {
            get;
            set;
        }

        //[JsonProperty("recurrenceRule")]
        public string RecurrenceRule
        {
            get;
            set;
        }

        //[JsonProperty("recurrenceException")]
        public string RecurrenceException
        {
            get;
            set;
        }

        //[JsonProperty("start")]
        public DateTime Start
        {
            get;
            set;
        }

        //[JsonProperty("end")]
        public DateTime End
        {
            get;
            set;
        }

        //[JsonProperty("startTimezone")]
        public string StartTimezone
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the employee id.
        /// </summary>
        /// <value>The employee id.</value>
        public Guid EmployeeId { get; set; }

        public int AndromedaSiteId { get; set; }

        /// <summary>
        /// Gets or sets the type of the task.
        /// </summary>
        /// <value>The type of the task.</value>
        public string TaskType { get; set; }
    }
}