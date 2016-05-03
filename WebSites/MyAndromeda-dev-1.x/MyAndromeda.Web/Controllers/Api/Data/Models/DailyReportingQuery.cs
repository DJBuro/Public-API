using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Data.Entity;
using System.Web.Http;
using MyAndromeda.Data.DailyReporting.Services;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using System.Threading.Tasks;
using MyAndromeda.Data.DataAccess.Chains;
using MyAndromeda.Framework.Dates;

namespace MyAndromeda.Web.Controllers.Api.Data
{

    public class DailyReportingQuery
    {
        private DateTime? from;
        public DateTime? From
        {
            get { return this.from; }
            set
            {
                if (value.HasValue)
                {
                    this.from = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day);
                }
                else
                {
                    this.from = value;
                }
            }
        }

        private DateTime? to;
        public DateTime? To
        {
            get { return this.to; }
            set
            {
                if (value.HasValue)
                {
                    this.to = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day);
                }
                else
                {
                    this.to = value;
                }
            }
        }
    }

}