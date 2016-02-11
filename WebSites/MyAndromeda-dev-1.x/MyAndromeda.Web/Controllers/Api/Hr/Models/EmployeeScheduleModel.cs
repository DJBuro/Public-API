using Kendo.Mvc.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Controllers.Api.Hr.Models
{
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


        public Guid EmployeeId { get; set; }
        public int AndromedaSiteId { get; set; }
        public string TaskType { get; set; }
    }
}