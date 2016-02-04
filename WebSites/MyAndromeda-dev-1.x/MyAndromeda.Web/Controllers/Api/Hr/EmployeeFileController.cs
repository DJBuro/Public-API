using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Logging;
using MyAndromeda.Web.Controllers.Api.Hr.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Converters;


namespace MyAndromeda.Web.Controllers.Api.Hr
{
    [RoutePrefix("hr/{chainId}/employees/{andromedaSiteId}")]
    public class EmployeeFileController : ApiController
    {
        private readonly IMyAndromedaLogger logger;
        private readonly DataWarehouseDbContext dbContext;
        private readonly DbSet<EmployeeRecord> employeeTable;

        public EmployeeFileController(IMyAndromedaLogger logger, DataWarehouseDbContext dbContext) 
        {
            this.dbContext = dbContext;
            this.employeeTable = this.dbContext.Set<EmployeeRecord>();
            this.logger = logger;
        }


        [HttpGet]
        [Route("list")]
        public async Task<List<EmployeeRecordModel>> List([FromUri]int andromedaSiteId) 
        {
            var query = this.employeeTable
                .Where(e => e.EmployeeStoreLinkRecords.Any(r => r.AdromedaSiteId == andromedaSiteId));

            var records = await query.ToListAsync();

            var models = records
                .Select((record) => record.ToViewModel())
                .ToList();

            //var models2 = models.Select((r) => r as dynamic);


            //var k = JsonConvert.SerializeObject(models);
            //var k2 = JsonConvert.SerializeObject(models2);

            //var converters = new List<Newtonsoft.Json.JsonConverter>();

            //var k3 = JsonConvert.SerializeObject(models2, new JsonSerializerSettings() { 
            //    CheckAdditionalContent=true,
            //    Converters = converters
            //});


            //var response = Request.CreateResponse(HttpStatusCode.OK, k2, Configuration.Formatters.JsonFormatter);

            return models; 

            //return response;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<EmployeeRecordModel> Get([FromUri]
                                                   int andromedaSiteId, [FromUri]
                                                   Guid id) 
        {
            var query = this.employeeTable.Where(e => e.EmployeeStoreLinkRecords.Any(k => k.AdromedaSiteId == andromedaSiteId));

            var record = await query.FirstOrDefaultAsync();

            return record.ToViewModel();
        }

        [HttpPost]
        [Route("update")]
        public async Task<EmployeeRecordModel> Update(
            [FromUri]
            int andromedaSiteId,
            [FromBody]
            EmployeeRecordModel model) 
        {

            var query = this.employeeTable
                            .Where(e => e.EmployeeStoreLinkRecords.Any(K => K.AdromedaSiteId == andromedaSiteId))
                            .Where(e => e.Id == model.Id);

            var dbItem = await query.FirstOrDefaultAsync();

            dbItem.UpdateProperties(model);

            var vm = dbItem.ToViewModel();

            await this.dbContext.SaveChangesAsync();

            return vm;
        }

        [HttpPost]
        [Route("create")]
        public async Task<EmployeeRecordModel> Create(
            [FromUri] int andromedaSiteId)
        {
            var content = await this.Request.Content.ReadAsStringAsync();
            EmployeeRecordModel result = null;
            try
            {
                var model = JsonConvert.DeserializeObject<EmployeeRecordModel>(content);

                var dbRecord = this.employeeTable.CreateDbItem(model, andromedaSiteId, true);

                await this.dbContext.SaveChangesAsync();

                result = dbRecord.ToViewModel();
            }
            catch (Exception ex)
            {
                this.logger.Error("failed to create EmployeeRecordModel");
                this.logger.Error(ex);   
                throw ex;
            }

            return result;
        }

        
    }

}