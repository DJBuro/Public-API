using Kendo.Mvc.UI;
using MyAndromeda.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace MyAndromeda.Web.Controllers.Api.Hr
{
    [RoutePrefix("hr/{chainId}/employees/{andromedaSiteId}/schedule/{employeeId}")]
    public class EmployeeSchedulingController : ApiController 
    {
        private readonly MyAndromeda.Data.DataWarehouse.Models.DataWarehouseDbContext dataWareHouseDbContext;

        private readonly IMyAndromedaLogger logger;

        public EmployeeSchedulingController(MyAndromeda.Data.DataWarehouse.Models.DataWarehouseDbContext dataWareHouseDbContext, IMyAndromedaLogger logger)
        { 
            this.logger = logger;
            this.dataWareHouseDbContext = dataWareHouseDbContext;
        }

        public static List<Models.EmployeeScheduleModel> Models = new List<Hr.Models.EmployeeScheduleModel>();

        [HttpPost]
        [Route("list")]
        public async Task<DataSourceResult> GetSchedule() 
        {
            string content = await this.Request.Content.ReadAsStringAsync();

            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(content);

            return new DataSourceResult()
            { 
                Total = 0,
                Data = Models
            };
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

            var entity = Models.FirstOrDefault(e => e.Id == model.Id);
            if (entity == null)
            {
                Models.Add(model);
            }
            else
            {
                Models.Remove(entity);
                Models.Add(model);
            }

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

            var entity = Models.FirstOrDefault(e => e.Id == model.Id);

            if (entity != null) 
            {
                Models.Remove(entity);
            }

            return entity;
        }

        //private IQueryable<Models.EmployeeScheduleModel> Query() 
        //{
            
        //}

    }
}