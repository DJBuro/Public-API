using System.Data.Entity;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Web.Controllers.Api.Hr.Models;

namespace MyAndromeda.Web.Controllers.Api.Hr
{
    [RoutePrefix("hr/{chainId}/employees/{andromedaSiteId}/schedule")]
    public class EmployeeSchedulingController : ApiController 
    {
        private readonly MyAndromeda.Data.DataWarehouse.Models.DataWarehouseDbContext dataWareHouseDbContext;

        private readonly IMyAndromedaLogger logger;

        private readonly DbSet<EmployeeSchedule> EmployeeSchedule;

        public EmployeeSchedulingController(MyAndromeda.Data.DataWarehouse.Models.DataWarehouseDbContext dataWareHouseDbContext, IMyAndromedaLogger logger)
        { 
            this.logger = logger;
            this.dataWareHouseDbContext = dataWareHouseDbContext;
            this.EmployeeSchedule = this.dataWareHouseDbContext.Set<EmployeeSchedule>();
        }


        [HttpPost]
        [Route("store-list")]
        public async Task<DataSourceResult> GetStoreSchedule([FromUri]int andromedaSiteId) 
        {
            string content = await this.Request.Content.ReadAsStringAsync();

            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(content);

            var models = this.EmployeeSchedule.Where(e => e.AndromedaSiteId == andromedaSiteId);

            var result = models.ToDataSourceResult(request, e=> e.ToModel());
            
            return result;
        }

        [HttpPost]
        [Route("list/{employeeId}")]
        public async Task<DataSourceResult> GetSchedule([FromUri]int AndromedaSiteId, [FromUri]Guid employeeId) 
        {
            string content = await this.Request.Content.ReadAsStringAsync();

            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(content);

            var models = this.EmployeeSchedule.Where(e => e.AndromedaSiteId == AndromedaSiteId && e.EmployeeRecordId == employeeId);

            var result = models.ToDataSourceResult(request, e=> e.ToModel());

            return result;
        }

        [HttpPost]
        [Route("update")]
        public async Task<object> Update() 
        {
            string content = await this.Request.Content.ReadAsStringAsync();

            Models.EmployeeScheduleModel model = null;
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

            var entity = await this.EmployeeSchedule.FirstOrDefaultAsync(e => e.Id == model.Id);
            if (entity == null)
            {
                entity = model.CreateEntiyFromModel();
                this.EmployeeSchedule.Add(entity);
            }
            else
            {
                entity.UpdateFromModel(model);
            }

            await this.dataWareHouseDbContext.SaveChangesAsync();

            //return model;
            //parse expects Data. 
            return new DataSourceResult()
            {
                Total = 1,
                Data = new[] { model }
            };
        }

        [HttpPost]
        [Route("destroy")]
        public async Task<object> Detroy()
        {
            string content = await this.Request.Content.ReadAsStringAsync();

            Models.EmployeeScheduleModel model = JsonConvert.DeserializeObject<Models.EmployeeScheduleModel>(content);

            var entity = await this.EmployeeSchedule.FirstOrDefaultAsync(e => e.Id == model.Id);

            if (entity != null) 
            {
                this.EmployeeSchedule.Remove(entity);
            }

            await this.dataWareHouseDbContext.SaveChangesAsync();

            return entity;
        }

    }
}