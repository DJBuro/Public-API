using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Http;
using Kendo.Mvc.Extensions;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Logging;
using MyAndromeda.Services.Orders.Events;
using MyAndromeda.Framework.Helpers;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using Kendo.Mvc.UI;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Web.Controllers.Api.Data
{

    public class Sort 
    {
        public string Dir { get; set; }
        public string Field { get; set; }
    }

}