using System.Data.Entity;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Web.Controllers.Api.Hr.Models;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using System.Collections.Generic;

namespace MyAndromeda.Web.Controllers.Api.Hr
{
    [RoutePrefix("hr/{chainId}/employees/{andromedaSiteId}/schedule")]
    public class EmployeeSchedulingController : ApiController
    {
        private readonly DataWarehouseDbContext dataWareHouseDbContext;

        private readonly IMyAndromedaLogger logger;

        private readonly DbSet<EmployeeSchedule> employeeScheduleTable;

        /* Utility services */
        private readonly INotifier notifier;
        private readonly ITranslator translator;

        public EmployeeSchedulingController(
            DataWarehouseDbContext dataWareHouseDbContext,
            IMyAndromedaLogger logger,
            INotifier notifier,
            ITranslator translator)
        {
            this.translator = translator;
            this.notifier = notifier;
            this.logger = logger;
            this.dataWareHouseDbContext = dataWareHouseDbContext;
            this.employeeScheduleTable = this.dataWareHouseDbContext.Set<EmployeeSchedule>();
        }


        [HttpPost]
        [Route("store-list")]
        public async Task<DataSourceResult> GetStoreSchedule([FromUri]int andromedaSiteId)
        {
            string content = await this.Request.Content.ReadAsStringAsync();

            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(content);

            IQueryable<EmployeeSchedule> models = this.employeeScheduleTable
                .Include(e => e.EmployeeRecord)
                .Where(e => e.AndromedaSiteId == andromedaSiteId);

            DataSourceResult result = models.ToDataSourceResult(request, e => e.ToModel());

            return result;
        }

        [HttpPost]
        [Route("list/{employeeId}")]

        public async Task<DataSourceResult> GetSchedule([FromUri]int andromedaSiteId, [FromUri]Guid employeeId)
        {
            string content = await this.Request.Content.ReadAsStringAsync();

            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(content);

            IQueryable<EmployeeSchedule> models = this.employeeScheduleTable
                .Include(e => e.EmployeeRecord)
                .Where(e => e.AndromedaSiteId == andromedaSiteId && e.EmployeeRecordId == employeeId);

            DataSourceResult result = models.ToDataSourceResult(request, e => e.ToModel());

            return result;
        }

        [HttpPost]
        [Route("update")]
        public async Task<object> Update()
        {
            string content = await this.Request.Content.ReadAsStringAsync();
            var result = new DataSourceResult()
            {
                Total = 0,
                Data = new List<EmployeeScheduleModel>()
            };
            EmployeeScheduleModel model = null;

            try
            {
                model = JsonConvert.DeserializeObject<Models.EmployeeScheduleModel>(content);
            }
            catch (Exception e)
            {
                this.logger.Error(e);
                throw e;
            }

            bool newModel = !model.Id.HasValue || model.Id == null;

            if (newModel)
            {
                model.Id = Guid.NewGuid();
            }

            EmployeeSchedule entity = await this.employeeScheduleTable.FirstOrDefaultAsync(e => e.Id == model.Id);
            
            if (entity == null)
            {
                entity = model.CreateEntiyFromModel();
                this.employeeScheduleTable.Add(entity);
            }
            else
            {
                entity.UpdateFromModel(model);
            }

            await this.dataWareHouseDbContext.SaveChangesAsync();

            //return model;
            //parse expects Data. 

            result.Total = 1;
            result.Data = new[] { model };
            return result;
        }

        [HttpPost]
        [Route("destroy")]
        public async Task<object> Detroy()
        {
            string content = await this.Request.Content.ReadAsStringAsync();

            Models.EmployeeScheduleModel model = JsonConvert.DeserializeObject<Models.EmployeeScheduleModel>(content);

            EmployeeSchedule entity = await this.employeeScheduleTable.FirstOrDefaultAsync(e => e.Id == model.Id);

            if (entity != null)
            {
                this.employeeScheduleTable.Remove(entity);
            }

            await this.dataWareHouseDbContext.SaveChangesAsync();

            return entity.ToModel();
        }

    }
}